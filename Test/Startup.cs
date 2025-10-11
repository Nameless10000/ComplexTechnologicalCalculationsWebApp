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
        services.ScanRepos();
        services.ScanServices();

        services.ConfigureDataBaseContexts(isTest: true);
    }
}