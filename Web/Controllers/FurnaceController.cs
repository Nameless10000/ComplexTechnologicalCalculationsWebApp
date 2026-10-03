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
        var calculationResult = await _service.Calculate(requestModel.GetRawText());
        return Content(calculationResult, "application/json");
    }
}