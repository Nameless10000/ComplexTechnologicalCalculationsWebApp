using BaseLib.Models2;
using Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Web.Controllers;

[Authorize]
public class GasDynamicController : Controller
{
    private readonly GasDynamicService _service;
    private ILogger<GasDynamicController> _logger;

    public GasDynamicController(ILogger<GasDynamicController> logger, GasDynamicService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpPut]
    public async Task<IActionResult> MarkCalculationAsPreset([FromQuery] int calculationId)
    {
        var creationResult = await _service.MarkCalculationAsPreset(calculationId);

        if (creationResult)
        {
            return Ok(new { message = "Preset created successfully" });
        }

        return BadRequest(new { message = "Preset creation failed" });
    }

    [HttpGet]
    public async Task<IActionResult> GetPreset()
    {
        var preset = await _service.GetPresetAsync();

        if (preset == null)
        {
            return Ok(new { message = "Preset not found" });
        }

        return Ok(new
        {
            input = JsonConvert.DeserializeObject<RequestModelV2>(preset.SerializedInput),
            output = JsonConvert.DeserializeObject<ResponseModelV2>(preset.SerializedOutput)
        });
    }

    [HttpPost]
    public async Task<IActionResult> Calculate([FromBody] RequestModelV2 requestModel)
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