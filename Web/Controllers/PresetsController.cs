using System.Security.Claims;
using System.Text.Json;
using Data.Services;
using Core.Models.Calculations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web.Controllers;

[Authorize, ApiController, Route("presets")]
public sealed class PresetsController(CalculationPresetService service) : ControllerBase
{
    private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    [HttpGet]
    public async Task<IActionResult> List([FromQuery] string? module)
    {
        var query = service.Own(UserId).AsNoTracking();
        if (module is not null) query = query.Where(x => x.Module == module);
        return Ok((await query.OrderBy(x => x.Name).Take(200).ToListAsync(HttpContext.RequestAborted)).Select(View));
    }
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id) => Ok(View(await service.Find(UserId, id, HttpContext.RequestAborted)));
    [HttpPost]
    public async Task<IActionResult> Create(PresetInput input)
    {
        var row = await Save(null, input);
        return CreatedAtAction(nameof(Get), new { id = row.Id }, View(row));
    }
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, PresetInput input) => Ok(View(await Save(id, input)));
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id) { await service.Delete(UserId, id, HttpContext.RequestAborted); return NoContent(); }
    private Task<CalculationPreset> Save(Guid? id, PresetInput input) => service.Save(UserId, id, input.Module, input.Name, input.Description, input.Payload.GetRawText(), HttpContext.RequestAborted);
    private static object View(CalculationPreset row) => new { row.Id, row.Module, row.Name, row.Description, row.CreatedAt, row.UpdatedAt, payload = JsonDocument.Parse(row.Payload).RootElement.Clone() };
}

public sealed record PresetInput(string Module, string Name, string? Description, JsonElement Payload);
