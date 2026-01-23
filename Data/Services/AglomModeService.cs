using AutoMapper;
using BaseLib;
using BaseLib.AglomMode;
using BaseLib.AglomMode.Models;
using BaseLib.SlagMode;
using BaseLib.SlagMode.Models;
using Core.Contexts;
using Core.Models.AglomMode;
using Core.Models.GasDynamic;
using Core.Models.SlagMode;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Data.Services;

public class AglomModeService(
    AgloDBContext dbContext,
    IHttpContextAccessor httpContextAccessor,
    AglomMode library,
    IMapper mapper,
    IConfiguration configuration)
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
      
//TODO: в случае ошибки по отдельности добавлять записи в базу
        var responseFromLib = library.Calculate(requestModel);
        var request = mapper.Map<AglomRequestData, AglomRequestDB>(requestModel);
        var response = mapper.Map<AglomResponseData, AglomResponseDB>(responseFromLib);
        request.AglomResponse = response;
        request.CreationDateTime = DateTime.UtcNow;

        await dbContext.AglomRequests.AddAsync(request);
        await dbContext.SaveChangesAsync();

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
            .Where(x => x.CreatorID == _currentUserId)
            .OrderByDescending(x => x.CreationDateTime)
            .AsQueryable();
    }
}