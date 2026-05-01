using BaseLib.AglomMode.Models;
using Contracts.Grpc;
using Contracts.History;
using Core.Contexts;
using Core.Models.AglomMode;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Data.Services;

public class AglomModeService(
    AgloDBContext dbContext,
    IHttpContextAccessor httpContextAccessor,
    AglomCalculator.AglomCalculatorClient calculatorClient,
    CalculationHistoryProducerService historyProducer)
{
    private HttpContext _httpContext => httpContextAccessor.HttpContext;
    private int _currentUserId => int.Parse(_httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

    public async Task<List<AglomRequestDB>> GetAllCalculationsAsync()
    {
        var calculations = await GetCalculationsQueryable()
            .ToListAsync();

        return calculations;
    }
  
    public async Task<AglomRequestDB?> GetCalculationAsync(int id)
    {
        var calulation = await GetCalculationsQueryable()
            .FirstOrDefaultAsync(x => x.Id == id);

        return calulation;
    }

    public async Task<AglomRequestDB?> GetPresetAsync()
    {
        var calculations = GetCalculationsQueryable();

        var preset = await calculations.FirstOrDefaultAsync();

        return preset;
    }

    public async Task<AglomResponseData> Calculate(AglomRequestData requestModel)
    {
        requestModel.UserId = _currentUserId;
        var requestJson = JsonConvert.SerializeObject(requestModel);
        var grpcResponse = await calculatorClient.CalculateAsync(new CalculationRequest { Json = requestJson });
        var responseFromLib = JsonConvert.DeserializeObject<AglomResponseData>(grpcResponse.Json) ?? new AglomResponseData();

        await historyProducer.PublishAsync(new CalculationHistoryEvent
        {
            Module = CalculationModules.AglomMode,
            UserId = _currentUserId,
            CreationDateTime = DateTime.UtcNow,
            RequestJson = requestJson,
            ResponseJson = JsonConvert.SerializeObject(responseFromLib)
        });

        return responseFromLib;
    }

    private IQueryable<AglomRequestDB> GetCalculationsQueryable()
    {
        return dbContext.AglomRequests
            .Include(x => x.ZolaOfCocksick)
            .Include(x => x.Cocksick)
            .Include(x => x.FluxAdditions)
            .Include(x => x.ShihtaComponents)
            .Include(x => x.AglomResponse)
            .Include(x => x.StartEnter)
            .Where(x => x.CreatorID == _currentUserId)
            .OrderByDescending(x => x.CreationDateTime)
            .AsQueryable();
    }
}
