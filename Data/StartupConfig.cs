using Core.Contexts;
using Core.Services;
using Data.Mapping;
using Microsoft.EntityFrameworkCore;
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
    /// Метод для конфигурации контекстов подключений БД
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection ConfigureDataBaseContexts(this IServiceCollection services,
        Dictionary<Type, string>? connStrings = null,
        bool isTest = false)
    {
        if (isTest || connStrings == null) // Регистрация InMemory контекстов БД для тестов
        {
            services.AddDbContext<AgloDBContext>(opt => opt.UseInMemoryDatabase(nameof(AgloDBContext)));
            services.AddDbContext<AuthDBContext>(opt => opt.UseInMemoryDatabase(nameof(AuthDBContext)));
            services.AddDbContext<GasDynamicDBContext>(opt => opt.UseInMemoryDatabase(nameof(GasDynamicDBContext)));
            services.AddDbContext<MatBalDBContext>(opt => opt.UseInMemoryDatabase(nameof(MatBalDBContext)));
            services.AddDbContext<SlagModeDBContext>(opt => opt.UseInMemoryDatabase(nameof(SlagModeDBContext)));
            services.AddDbContext<TBalDBContext>(opt => opt.UseInMemoryDatabase(nameof(TBalDBContext)));
            services.AddDbContext<TModeDBContext>(opt => opt.UseInMemoryDatabase(nameof(TModeDBContext)));

            return services;
        }

        var agloConnStr = connStrings[typeof(AgloDBContext)];
        var authConnStr = connStrings[typeof(AuthDBContext)];
        var gasDynamicConnStr = connStrings[typeof(GasDynamicDBContext)];
        var matBalConnStr = connStrings[typeof(MatBalDBContext)];
        var slagModeConnStr = connStrings[typeof(SlagModeDBContext)];
        var tBalConnStr = connStrings[typeof(TBalDBContext)];
        var tModeConnStr = connStrings[typeof(TModeDBContext)];

        services.AddDbContext<AgloDBContext>(opt => opt.UseNpgsql(agloConnStr));
        services.AddDbContext<AuthDBContext>(opt => opt.UseNpgsql(authConnStr));
        services.AddDbContext<GasDynamicDBContext>(opt => opt.UseNpgsql(gasDynamicConnStr));
        services.AddDbContext<MatBalDBContext>(opt => opt.UseNpgsql(matBalConnStr));
        services.AddDbContext<SlagModeDBContext>(opt => opt.UseNpgsql(slagModeConnStr));
        services.AddDbContext<TBalDBContext>(opt => opt.UseNpgsql(tBalConnStr));
        services.AddDbContext<TModeDBContext>(opt => opt.UseNpgsql(tModeConnStr));

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

        services.AddTransient<SimpleLoggerService>();
        
        
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

    /// <summary>
    /// Сканирование репозиториев для добавления в DI
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection ScanMathLibs(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromApplicationDependencies()
            .AddClasses(classes => classes
                .InNamespaces("BaseLib")
                .Where(type => type.GetInterfaces().FirstOrDefault() is { Name: "IMathLibrary`2" }))
            .AsSelf()
            .WithSingletonLifetime());

        return services;
    }

    /// <summary>
    /// Регистрация автомаппера
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection ConfigMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(opt => opt.AddProfile<MapperProfile>());

        return services;
    }
}