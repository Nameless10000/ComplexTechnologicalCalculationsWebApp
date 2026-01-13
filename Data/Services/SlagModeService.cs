using System.Security.Claims;
using AutoMapper;
using BaseLib;
using BaseLib.SlagMode;
using BaseLib.SlagMode.Models;
using Core.Contexts;
using Core.Models.GasDynamic;
using Core.Models.SlagMode;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Data.Services;

public class SlagModeService(
    SlagModeDBContext dbContext,
    IHttpContextAccessor httpContextAccessor,
    SlagMode library,
    IMapper mapper,
    IConfiguration configuration)
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
        requestModel.User = new UserAuthData
        {
            UserName = configuration["Authorization:UserName"],
            Password = configuration["Authorization:Password"],
        };
        
//TODO: в случае ошибки по отдельности добавлять записи в базу
        var responseFromLib = library.Calculate(requestModel);
        var request = mapper.Map<RequestData,Request>(requestModel);
        var response = mapper.Map<ResponseData,Response>(responseFromLib);
        response.Request = request;
        response.CreationDateTime = DateTime.UtcNow;

        await dbContext.Responses.AddAsync(response);
        await dbContext.SaveChangesAsync();

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