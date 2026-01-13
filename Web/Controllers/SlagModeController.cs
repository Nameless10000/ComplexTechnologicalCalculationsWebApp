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
            //var calculationResult = await _service.Calculate(requestModel);
            //return Ok(new { data = calculationResult });
            return Ok(new { data = new ResponseData
            {
                SlagBasicity1 = 1.1187089715536105,
                SlagBasicity2 = 1.3208971553610502,
                SlagBasicity3 = 1.0062929777036882,
                SlagBasicityKulikov = 1.2446003148807527,
                SlagOut = 263.9732909819013,
                MaterialCons = 1568.6164909614838,
                TotalAglo = 927.4,
                PropAglo23 = 0.26702552316438855,
                PropAglo4 = 0.29387927906132816,
                PropSsgpo = 0.3439579049231886,
                PropLeb = 0.03308334341357204,
                PropKach = 0.03272045482037015,
                PropMix = 0.029333494617152536,
                PropOre = 0,
                PropWeldSlag = 0,
                PropBfAddict = 0,
                PropMinInclude = 0,
                Viscosity_1400 = 6.703502526969734,
                Viscosity_1450 = 4.030430354464176,
                Viscosity_1500 = 2.77640269624368,
                Viscosity_1550 = 2.1129864804405654,
                Temp_7_Puaz = 1396.3855818462755,
                Gradient_7_25 = 0.2225587107350592,
                Gradient_1400_1500 = 0.03927099830726054,
                SlagTemperature = 1488.3385600000001,
                SlagTemperature_25Puaz = 1315.5080464734733,
                CurrSlagViscosity = 2.9982111721413673,
                BalSlagMass = 263.9732909819013,
                CaOBalSlagMass = 256.15142487066544,
                TotalSInOre = 2.4246469315316554,
                SActivity = 5.060329763376613,
                SDistribution = 0,
                SContentInCastIron = 0.016,
                CastIronTemp = 1450
            } });
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