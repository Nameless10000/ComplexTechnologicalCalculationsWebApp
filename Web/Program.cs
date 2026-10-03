using System.Diagnostics;
using AutoMapper;
using BaseLib.SlagMode.Models;
using Contracts.Grpc;
using Core.Contexts;
using Core.Models.Auth;
using Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Web.Seed;
using Web.Infrastructure;
using Data.Infrastructure;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community; // Educational project.

// Add services to the container.
builder.Services.AddControllersWithViews(options => options.Filters.Add<ApiResultFilter>()).AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);
builder.Services.AddExceptionHandler<ApiExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddHealthChecks()
    .AddCheck<PostgreSqlHealthCheck>("postgresql", tags: ["ready"], timeout: TimeSpan.FromSeconds(5))
    .AddCheck<KafkaHealthCheck>("kafka", tags: ["ready"], timeout: TimeSpan.FromSeconds(5))
    .AddCheck<CalculationGrpcHealthCheck>("grpc", tags: ["ready"], timeout: TimeSpan.FromSeconds(15));

var conStrings = new Dictionary<Type, string>();
conStrings[typeof(AgloDBContext)] = builder.Configuration.GetConnectionString("AgloConnectionString")!;
conStrings[typeof(AuthDBContext)] = builder.Configuration.GetConnectionString("AuthConnectionString")!;
conStrings[typeof(GasDynamicDBContext)] = builder.Configuration.GetConnectionString("GasDynamicConnectionString")!;
conStrings[typeof(MatBalDBContext)] = builder.Configuration.GetConnectionString("MatBalConnectionString")!;
conStrings[typeof(SlagModeDBContext)] = builder.Configuration.GetConnectionString("SlagModeConnectionString")!;
conStrings[typeof(FurnaceDBContext)] = builder.Configuration.GetConnectionString("FurnaceConnectionString")!;
conStrings[typeof(TBalDBContext)] = builder.Configuration.GetConnectionString("TBalConnectionString")!;
conStrings[typeof(TModeDBContext)] = builder.Configuration.GetConnectionString("TModeConnectionString")!;

var serverDomain = builder.Configuration.GetSection("ExternalServer");
builder.Services.Configure<ExternalServerDomain>(serverDomain);

builder.Services.AddHttpContextAccessor();

builder.Services.ConfigureDataBaseContexts(conStrings);
builder.Services.ConfigureKafka(builder.Configuration);
builder.Services.ScanServices();
builder.Services.AddScoped<OutboxDispatcher>();
builder.Services.AddHostedService<OutboxWorker>();
builder.Services.ScanRepos();
builder.Services.ConfigMapper();

builder.Services.AddGrpcClient<AglomCalculator.AglomCalculatorClient>(options =>
{
    options.Address = new Uri(builder.Configuration["GrpcServices:AglomMode"]!);
});
builder.Services.AddGrpcClient<GasDynamicCalculator.GasDynamicCalculatorClient>(options =>
{
    options.Address = new Uri(builder.Configuration["GrpcServices:GasDynamic"]!);
});
builder.Services.AddGrpcClient<SlagCalculator.SlagCalculatorClient>(options =>
{
    options.Address = new Uri(builder.Configuration["GrpcServices:SlagMode"]!);
});

builder.Services.AddGrpcClient<FurnaceService.FurnaceServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration["GrpcServices:FurnaceService"]!);
});

builder.Services.AddIdentity<User, Role>(options =>
    {
        // ����������� ����� ������
        options.Password.RequiredLength = 3;

        // ����� ��, ����� ������ �������� ���� �� ���� �����������-�������� ������ (��������, !, @, #)
        options.Password.RequireNonAlphanumeric = false;

        // ����� ��, ����� ������ �������� ���� �� ���� ��������� �����
        options.Password.RequireUppercase = false;

        // ����� �������� ������ ���������:
        // options.Password.RequireDigit = true;       // ��������� �����
        // options.Password.RequireLowercase = true;   // ��������� ���� �� ���� �������� �����
        // options.User.RequireUniqueEmail = true;    // ��������� ���������� email ��� �����������
    })
    .AddEntityFrameworkStores<
        AuthDBContext>() // ���������, ��� Identity ����� ������������ AuthDBContext ��� �������� ������������� � �����
    .AddDefaultTokenProviders(); // ��������� ���������� ������� ��� ������������� email, ������ ������ � �.�.

builder.Services.ConfigureApplicationCookie(options =>
{
    // ����, �� ������� ���������������� ������������, ���� �� �� �����������
    options.LoginPath = "/Auth/Authorize";

    // ���� ��� ������ ������������ �� �������
    options.LogoutPath = "/Auth/Logout";

    // ������ cookie ���������� ������ ��� HTTP-��������, ����� �� ������ ���� ��������� ����� JavaScript
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;

    // ����� ����� cookie � ����� ����� ������� ������������ ������������� ������
    options.ExpireTimeSpan = TimeSpan.FromHours(1);

    options.Events.OnRedirectToLogin = context =>
    {
        if (IsApiRequest(context.Request))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        }

        context.Response.Redirect(context.RedirectUri);
        return Task.CompletedTask;
    };

    options.Events.OnRedirectToAccessDenied = context =>
    {
        if (IsApiRequest(context.Request))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return Task.CompletedTask;
        }

        context.Response.Redirect(context.RedirectUri);
        return Task.CompletedTask;
    };

    // ����� �������� ������ ���������:
    // options.Cookie.Name = "MyAppAuthCookie";  // ��� cookie
    // options.SlidingExpiration = true;         // ��������� ���� �������� cookie ��� ���������� ������������
});

var app = builder.Build();
app.UseExceptionHandler();
app.Use((context, next) => { context.Request.EnableBuffering(); return next(context); });
app.UseStatusCodePages(async statusContext =>
{
    var context = statusContext.HttpContext;
    var status = context.Response.StatusCode;
    var code = status switch { 401 => "UNAUTHORIZED", 403 => "FORBIDDEN", 404 => "NOT_FOUND", _ => "REQUEST_ERROR" };
    var message = status switch { 401 => "Войдите в систему.", 403 => "Доступ запрещён.", 404 => "Ресурс не найден.", _ => "Не удалось выполнить запрос." };
    await context.Response.WriteAsJsonAsync(ApiError.Create(context, code, message));
});
try
{
    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseCors(x =>
    {
        x.AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins("http://localhost:3000")
            .AllowCredentials()
            .WithExposedHeaders("X-Calculation-Id", "X-Correlation-Id", "X-History-Status");
    });

    using var scope = app.Services.CreateScope();

    var agloDB = scope.ServiceProvider.GetRequiredService<AgloDBContext>();
    var authDb = scope.ServiceProvider.GetRequiredService<AuthDBContext>();
    var gasDb = scope.ServiceProvider.GetRequiredService<GasDynamicDBContext>();
    var matDb = scope.ServiceProvider.GetRequiredService<MatBalDBContext>();
    var slagDb = scope.ServiceProvider.GetRequiredService<SlagModeDBContext>();
    var furnaceDb = scope.ServiceProvider.GetRequiredService<FurnaceDBContext>();
    var tbalDb = scope.ServiceProvider.GetRequiredService<TBalDBContext>();
    var tmodeDb = scope.ServiceProvider.GetRequiredService<TModeDBContext>();
    var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

    agloDB.Database.Migrate();
    await AglomDefaultPresetSeeder.SeedAsync(agloDB, mapper);
    authDb.Database.Migrate();
    gasDb.Database.Migrate();
    await GasDynamicDefaultPresetSeeder.SeedAsync(gasDb);
    matDb.Database.Migrate();
    slagDb.Database.Migrate();
    await SlagModeDefaultPresetSeeder.SeedAsync(slagDb, mapper);
    tbalDb.Database.Migrate();
    tmodeDb.Database.Migrate();
    furnaceDb.Database.Migrate();

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();
    HealthCheckOptions HealthOptions(bool live) => new()
    {
        Predicate = live ? _ => false : registration => registration.Tags.Contains("ready"),
        ResponseWriter = async (context, report) => await context.Response.WriteAsJsonAsync(new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(x => new { name = x.Key, status = x.Value.Status.ToString(), description = x.Value.Description, durationMs = x.Value.Duration.TotalMilliseconds })
        })
    };
    app.MapHealthChecks("/health", HealthOptions(false));
    app.MapHealthChecks("/health/ready", HealthOptions(false));
    app.MapHealthChecks("/health/live", HealthOptions(true));

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception ex)
{
    app.Logger.LogCritical(ex, "Application startup failed.");
    throw;
}

static bool IsApiRequest(HttpRequest request)
{
    if (request.Headers.TryGetValue("X-Forwarded-Prefix", out var prefix)
        && prefix.Any(value => string.Equals(value, "/api", StringComparison.OrdinalIgnoreCase)))
    {
        return true;
    }

    return request.Path.StartsWithSegments("/Auth")
           || request.Path.StartsWithSegments("/GasDynamic")
           || request.Path.StartsWithSegments("/AglomMode")
           || request.Path.StartsWithSegments("/SlagMode")
           || request.Path.StartsWithSegments("/Furnace")
           || request.Path.StartsWithSegments("/calculations")
           || request.Path.StartsWithSegments("/presets");
}
