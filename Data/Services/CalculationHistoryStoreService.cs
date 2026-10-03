using System.Security.Claims;
using Contracts.History;
using Core.Contexts;
using Core.Models.Calculations;
using Data.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Data.Services;

public class CalculationHistoryStoreService(AuthDBContext db, IHttpContextAccessor accessor)
{
    public int UserId => int.TryParse(accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : 0;

    public async Task SaveAsync(CalculationHistoryEvent historyEvent, CancellationToken token = default)
    {
        var context = accessor.HttpContext;
        var metadata = CalculationGrpcMetadata.Request("", historyEvent.Module, context);
        Guid? sourceId = null;
        if (context?.Request.Headers.TryGetValue("X-Source-Calculation-Id", out var source) == true)
        {
            if (!Guid.TryParse(source, out var parsed) || !await db.CalculationHistory.AnyAsync(x => x.Id == parsed && x.UserId == UserId, token))
                throw new ArgumentException("Исходный расчёт не найден.");
            sourceId = parsed;
        }
        historyEvent.Id = Guid.NewGuid();
        historyEvent.CorrelationId = metadata.CorrelationId;
        var request = JObject.Parse(historyEvent.RequestJson);
        request.Properties().Where(x => x.Name.Equals("User", StringComparison.OrdinalIgnoreCase)).ToList().ForEach(x => x.Remove());
        historyEvent.RequestJson = request.ToString(Formatting.None);
        var record = new CalculationRecord
        {
            Id = historyEvent.Id, UserId = historyEvent.UserId, UserName = context?.User.Identity?.Name ?? "Гость",
            Module = historyEvent.Module, RequestId = metadata.RequestId, CorrelationId = metadata.CorrelationId,
            RequestJson = historyEvent.RequestJson, ResponseJson = historyEvent.ResponseJson, CreatedAt = historyEvent.CreationDateTime,
            SourceCalculationId = sourceId
        };
        db.CalculationHistory.Add(record);
        db.CalculationOutbox.Add(new OutboxMessage
        {
            Id = Guid.NewGuid(), Calculation = record, CalculationId = record.Id,
            Payload = JsonConvert.SerializeObject(historyEvent), CreatedAt = record.CreatedAt, NextRetryAt = record.CreatedAt
        });
        // EF commits the history and event in the same transaction.
        await db.SaveChangesAsync(token);
        if (context is not null)
        {
            context.Response.Headers["X-Calculation-Id"] = record.Id.ToString();
            context.Response.Headers["X-Correlation-Id"] = record.CorrelationId;
            context.Response.Headers["X-History-Status"] = record.Status;
        }
    }
}
