using System.Security.Claims;
using BaseLib;
using BaseLib.Models2;
using Core.Contexts;
using Core.Models.GasDynamic;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Data.Services;

public class GasDynamicService(
    GasDynamicDBContext dbContext,
    IHttpContextAccessor httpContextAccessor,
    BlastFurnaceSmeltingGasDynamicModeXLLibrary library)
{
    private HttpContext _httpContext => httpContextAccessor.HttpContext;
    private int _currentUserId => int.Parse(_httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

    public async Task<List<CalculationModel>> GetAllCalculationsAsync()
    {
        var calculations = await GetCalculationsQueryable()
            .ToListAsync();

        return calculations;
    }

    public async Task<CalculationModel?> GetCalculationAsync(int id)
    {
        var calulation = await GetCalculationsQueryable()
            .FirstOrDefaultAsync(x => x.Id == id);

        return calulation;
    }

    public async Task<CalculationModel?> GetPresetAsync()
    {
        var calculations = GetCalculationsQueryable();

        var preset = await calculations.FirstOrDefaultAsync(x => x.IsPreset)
                     ?? await calculations.FirstOrDefaultAsync();

        return preset;
    }

    public async Task<ResponseModelV2> Calculate(RequestModelV2 requestModel)
    {
        var response = library.Calculate(requestModel);

        var calculation = new CalculationModel
        {
            OwnerId = _currentUserId,
            CreationDateTime = DateTime.UtcNow,
            CreatorID = _currentUserId,
            SerializedInput = JsonConvert.SerializeObject(requestModel),
            SerializedOutput = JsonConvert.SerializeObject(response)
        };

        await dbContext.CalculationModels.AddAsync(calculation);
        await dbContext.SaveChangesAsync();

        return response;
    }


    public async Task<bool> MarkCalculationAsPreset(int calculationId)
    {
        var calculations = GetCalculationsQueryable();

        var targetCalculation = await calculations.FirstOrDefaultAsync(x => x.Id == calculationId);

        if (targetCalculation == null || targetCalculation.IsPreset)
        {
            return false;
        }

        var currentPreset = await calculations.FirstOrDefaultAsync(x => x.IsPreset);

        if (currentPreset != null)
        {
            currentPreset.IsPreset = false;
            dbContext.Update(currentPreset);
        }

        targetCalculation.IsPreset = true;

        dbContext.CalculationModels.Update(targetCalculation);
        await dbContext.SaveChangesAsync();

        return true;
    }

    private IQueryable<CalculationModel> GetCalculationsQueryable()
    {
        return dbContext.CalculationModels
            .Where(x => x.OwnerId == _currentUserId)
            .OrderByDescending(x => x.CreationDateTime)
            .AsQueryable();
    }
}