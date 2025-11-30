using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class GasDynamicController : Controller
{
    private ILogger<GasDynamicController> _logger;

    public GasDynamicController(ILogger<GasDynamicController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePreset()
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    public async Task<IActionResult> GetPreset([FromQuery] int presetId)
    {
    }

    [HttpPost]
    public async Task<IActionResult> Calculate()
    {
    }

    [HttpGet]
    public async Task<IActionResult> LoadCalculation([FromQuery] int calculationId)
    {
    }

    [HttpGet]
    public async Task<IActionResult> GetCalculationsHistory()
    {
    }
}