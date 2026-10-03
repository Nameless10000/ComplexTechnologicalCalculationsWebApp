using System.Security.Claims;
using Core.Contexts;
using Core.Models.Calculations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Web.Controllers;

[Authorize, ApiController, Route("calculations")]
public class CalculationsController(AuthDBContext db) : ControllerBase
{
    [HttpGet("compare")]
    public async Task<IActionResult> Compare([FromQuery] Guid leftId, [FromQuery] Guid rightId,
        [FromServices] Data.Services.CalculationComparisonService service) => Ok(service.Compare(await Find(leftId), await Find(rightId)));
    private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    [HttpGet("{id:guid}/export")]
    public async Task<IActionResult> Export(Guid id, [FromQuery] string format,
        [FromServices] Data.Services.CalculationReportExporterService exporter)
    {
        var row = await Find(id);
        return File(exporter.Export(row, format), format == "pdf" ? "application/pdf" : "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{row.Module}-{row.Id}.{format}");
    }
    [HttpPost("{id:guid}/transition/slag-mode")]
    public async Task<IActionResult> Transition(Guid id,
        [FromBody(EmptyBodyBehavior = Microsoft.AspNetCore.Mvc.ModelBinding.EmptyBodyBehavior.Allow)] JsonElement? existing,
        [FromServices] Data.Services.AglomSlagTransitionService service)
    {
        var transition = service.Map(await Find(id), existing.HasValue ? Newtonsoft.Json.Linq.JObject.Parse(existing.Value.GetRawText()) : null);
        return Ok(new { transition.SourceCalculationId, targetModule = "slag-mode", input = JsonDocument.Parse(transition.Input.ToString()).RootElement.Clone(), transition.MappedFields,
            message = "Проверьте расход агломерата, кокс, чугун и шлак перед запуском расчёта." });
    }
    private IQueryable<CalculationRecord> Own => db.CalculationHistory.AsNoTracking().Where(x => x.UserId == UserId);

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] string? module, [FromQuery] int skip = 0, [FromQuery] int take = 50)
    {
        if (skip < 0 || take is < 1 or > 100) throw new ArgumentException("Проверьте параметры страницы.");
        var query = module is null ? Own : Own.Where(x => x.Module == module);
        var rows = await query.OrderByDescending(x => x.CreatedAt).Skip(skip).Take(take).ToListAsync(HttpContext.RequestAborted);
        return Ok(rows.Select(View));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id) => Ok(View(await Find(id)));

    private async Task<CalculationRecord> Find(Guid id) => await Own.FirstOrDefaultAsync(x => x.Id == id, HttpContext.RequestAborted) ?? throw new KeyNotFoundException();
    private static object View(CalculationRecord row) => new
    {
        row.Id, row.Module, row.UserName, row.CreatedAt, row.Status, row.RequestId, row.CorrelationId, row.SourceCalculationId,
        input = JsonDocument.Parse(row.RequestJson).RootElement.Clone(), output = JsonDocument.Parse(row.ResponseJson).RootElement.Clone()
    };
}
