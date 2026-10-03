using Contracts.History;
using Core.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Data.Infrastructure;

public class OutboxDispatcher(AuthDBContext db, IHistoryEventPublisher publisher, ILogger<OutboxDispatcher> logger)
{
    public async Task<int> DispatchAsync(CancellationToken token)
    {
        await using var transaction = db.Database.IsRelational() ? await db.Database.BeginTransactionAsync(token) : null;
        var now = DateTime.UtcNow;
        var query = db.Database.IsNpgsql()
            ? db.CalculationOutbox.FromSqlInterpolated($"SELECT * FROM \"CalculationOutbox\" WHERE \"ProcessedAt\" IS NULL AND \"NextRetryAt\" <= {now} ORDER BY \"CreatedAt\" LIMIT 20 FOR UPDATE SKIP LOCKED")
            : db.CalculationOutbox.Where(x => x.ProcessedAt == null && x.NextRetryAt <= now).OrderBy(x => x.CreatedAt).Take(20);
        var messages = await query.ToListAsync(token);
        foreach (var message in messages)
        {
            message.Attempts++;
            try
            {
                var historyEvent = JsonConvert.DeserializeObject<CalculationHistoryEvent>(message.Payload) ?? throw new InvalidOperationException("Empty outbox payload.");
                await publisher.PublishAsync(historyEvent, token);
                message.ProcessedAt = DateTime.UtcNow;
                message.LastError = null;
            }
            catch (OperationCanceledException) when (token.IsCancellationRequested) { throw; }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Outbox publication failed for {Id}", message.Id);
                message.LastError = ex.Message.Length > 1000 ? ex.Message[..1000] : ex.Message;
                message.NextRetryAt = DateTime.UtcNow.AddSeconds(Math.Min(60, Math.Pow(2, Math.Min(message.Attempts, 6))));
            }
        }
        await db.SaveChangesAsync(token);
        if (transaction is not null) await transaction.CommitAsync(token);
        return messages.Count;
    }
}
