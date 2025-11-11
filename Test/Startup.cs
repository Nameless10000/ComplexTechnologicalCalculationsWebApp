using BaseLib.SlagMode.Models;
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
        var serverDomain = "localhost:7258";
        services.Configure<ExternalServerDomain>(options =>
        {
            options.Domain = serverDomain;
        });
        
        services.ScanRepos();
        services.ScanServices();

        services.ConfigureDataBaseContexts(isTest: true);
    }
}