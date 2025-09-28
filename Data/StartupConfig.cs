using Microsoft.Extensions.DependencyInjection;

namespace Data;

/// <summary>
/// Статик класс для вынесения логики конфигурации сервисов
/// </summary>
public static class StartupConfig
{
    /// <summary>
    /// Тут хранится текущая среда выполнения (Development, Testing, Production ...)
    /// </summary>
    private static readonly string _currentEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")!;

    /// <summary>
    /// Метод для конфигурации контекстов полдключений БД
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection ConfigureDataBaseContexts(this IServiceCollection services)
    {
        //services.AddDbContext<IDbContext>(opt => opt.UseNpgsql(...));


        return services;
    }

    /// <summary>
    /// Сканирование сервисов для добавления в DI
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection ScanServices(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromApplicationDependencies()
            .AddClasses(classes => classes
                .InNamespaces("Data")
                .Where(type => type.Name.Contains("Service")))
            .AsSelf()
            .WithTransientLifetime());

        return services;
    }

    /// <summary>
    /// Сканирование репозиториев для добавления в DI
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection ScanRepos(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromApplicationDependencies()
            .AddClasses(classes => classes
                .InNamespaces("Core")
                .Where(type => type.Name.Contains("Repository")))
            .AsSelf()
            .WithTransientLifetime());

        return services;
    }
}