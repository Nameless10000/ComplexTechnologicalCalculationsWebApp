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
    private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
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
