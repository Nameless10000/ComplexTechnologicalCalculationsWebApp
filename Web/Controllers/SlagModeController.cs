using BaseLib.SlagMode.Models;
using Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;
//[Authorize]
public class SlagModeController : Controller
{
        private readonly SlagModeService _service;
    private ILogger<SlagModeController> _logger;

    public SlagModeController(ILogger<SlagModeController> logger, SlagModeService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetChargeComponents()
    {
        var components = await _service.GetChargeComponents();
        return Ok(components.Count == 0
            ? new { message = "No charge components found" }
            : components);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetPreset()
    {
        var preset = await _service.GetPresetAsync();

        if (preset == null)
        {
            return Ok(new { message = "Preset not found" });
        }

        return Ok(new {data = preset});
    }

    [HttpPost]
    public async Task<IActionResult> Calculate([FromBody] RequestData requestModel)
    {
        try
        {
            var calculationResult = await _service.Calculate(requestModel);
            return Ok(new { data = calculationResult });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> LoadCalculation([FromQuery] int calculationId)
    {
        var calulcation = await _service.GetCalculationAsync(calculationId);

        if (calulcation is null)
        {
            return Ok(new { message = "Calculation not found" });
        }

        return Ok(calulcation);
    }

    [HttpGet]
    public async Task<IActionResult> GetCalculationsHistory()
    {
        var calculations = await _service.GetAllCalculationsAsync();

        if (calculations.Count == 0)
        {
            return Ok(new { message = "No calculations found" });
        }

        return Ok(calculations);
    }
}