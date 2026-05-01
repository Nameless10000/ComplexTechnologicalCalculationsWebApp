using System.Security.Claims;
using AutoMapper;
using BaseLib.SlagMode.Models;
using Contracts.Grpc;
using Contracts.History;
using Core.Contexts;
using Core.Models.SlagMode;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Data.Services;

public class SlagModeService(
    SlagModeDBContext dbContext,
    IHttpContextAccessor httpContextAccessor,
    IMapper mapper,
    SlagCalculator.SlagCalculatorClient calculatorClient,
    CalculationHistoryProducerService historyProducer)
{
    private HttpContext _httpContext => httpContextAccessor.HttpContext;
    private int _currentUserId => int.Parse(_httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

    public async Task<List<Response>> GetAllCalculationsAsync()
    {
        var calculations = await GetCalculationsQueryable()
            .ToListAsync();

        return calculations;
    }


    public async Task<List<InputChargeComponentsForCalc>> GetChargeComponents()
    {
        var components = await dbContext.ChargeComponents.ToListAsync();
        var res = components
            .Select(x => mapper.Map<ChargeComponent, InputChargeComponentsForCalc>(x))
            .ToList();
        return res;
    }
    
    public async Task<Response?> GetCalculationAsync(int id)
    {
        var calulation = await GetCalculationsQueryable()
            .FirstOrDefaultAsync(x => x.Id == id);

        return calulation;
    }

    public async Task<Response?> GetPresetAsync()
    {
        var calculations = GetCalculationsQueryable();

        var preset = await calculations.FirstOrDefaultAsync();

        return preset;
    }

    public async Task<ResponseData> Calculate(RequestData requestModel)
    {
        var requestJson = JsonConvert.SerializeObject(requestModel);
        var grpcResponse = await calculatorClient.CalculateAsync(new CalculationRequest { Json = requestJson });
        var responseFromLib = JsonConvert.DeserializeObject<ResponseData>(grpcResponse.Json) ?? new ResponseData();

        await historyProducer.PublishAsync(new CalculationHistoryEvent
        {
            Module = CalculationModules.SlagMode,
            UserId = _currentUserId,
            CreationDateTime = DateTime.UtcNow,
            RequestJson = requestJson,
            ResponseJson = JsonConvert.SerializeObject(responseFromLib)
        });

        return responseFromLib;
    }

    private IQueryable<Response> GetCalculationsQueryable()
    {
        return dbContext.Responses
            .Include(x => x.Request.CastIron)
            .Include(x => x.Request.Slag)
            .Include(x => x.Request.InputCoke)
            .Include(x => x.Request.Components)
            .Where(x => x.CreatorID == _currentUserId)
            .OrderByDescending(x => x.CreationDateTime)
            .AsQueryable();
    }
}
