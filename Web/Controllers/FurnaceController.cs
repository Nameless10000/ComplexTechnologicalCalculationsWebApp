using System.Text.Json;
using Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class FurnaceController : Controller
{
    private readonly FurnaceCalculationService _service;
    private readonly ILogger<FurnaceController> _logger;

    public FurnaceController(
        ILogger<FurnaceController> logger,
        FurnaceCalculationService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Calculate(
        [FromBody] JsonElement requestModel)
    {
        try
        {
            var requestJson = requestModel.GetRawText();

            var calculationResult =
                await _service.Calculate(requestJson);

            return Content(
                calculationResult,
                "application/json"
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return BadRequest(new { message = ex.Message });
        }
    }
}