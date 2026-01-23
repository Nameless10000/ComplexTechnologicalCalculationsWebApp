using BaseLib.SlagMode.Models;
using BaseLib;
using Data;
using Microsoft.Extensions.DependencyInjection;

namespace Test;

public class Startup
{
    /// <summary>
    /// Конфигурация DI для тестовой среды
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {
        var serverDomain = "localhost:44324";
        var AglomserverDomain = "localhost:5296";
        services.Configure<ExternalServerDomain>(options =>
        {
            options.Domain = serverDomain;
            options.AglomDomain = AglomserverDomain;
        });
        
        services.ScanRepos();
        services.ScanServices();
        services.ScanMathLibs();

        services.ConfigureDataBaseContexts(isTest: true);
    }
}