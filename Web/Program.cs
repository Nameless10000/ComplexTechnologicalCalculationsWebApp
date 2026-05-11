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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var conStrings = new Dictionary<Type, string>();
conStrings[typeof(AgloDBContext)] = builder.Configuration.GetConnectionString("AgloConnectionString")!;
conStrings[typeof(AuthDBContext)] = builder.Configuration.GetConnectionString("AuthConnectionString")!;
conStrings[typeof(GasDynamicDBContext)] = builder.Configuration.GetConnectionString("GasDynamicConnectionString")!;
conStrings[typeof(MatBalDBContext)] = builder.Configuration.GetConnectionString("MatBalConnectionString")!;
conStrings[typeof(SlagModeDBContext)] = builder.Configuration.GetConnectionString("SlagModeConnectionString")!;
conStrings[typeof(TBalDBContext)] = builder.Configuration.GetConnectionString("TBalConnectionString")!;
conStrings[typeof(TModeDBContext)] = builder.Configuration.GetConnectionString("TModeConnectionString")!;

var serverDomain = builder.Configuration.GetSection("ExternalServer");
builder.Services.Configure<ExternalServerDomain>(serverDomain);

builder.Services.AddHttpContextAccessor();

builder.Services.ConfigureDataBaseContexts(conStrings);
builder.Services.ConfigureKafka(builder.Configuration);
builder.Services.ScanServices();
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
try
{
    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseCors(x =>
    {
        x.AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins("http://localhost:3000")
            .AllowCredentials();
    });

    using var scope = app.Services.CreateScope();

    var agloDB = scope.ServiceProvider.GetRequiredService<AgloDBContext>();
    var authDb = scope.ServiceProvider.GetRequiredService<AuthDBContext>();
    var gasDb = scope.ServiceProvider.GetRequiredService<GasDynamicDBContext>();
    var matDb = scope.ServiceProvider.GetRequiredService<MatBalDBContext>();
    var slagDb = scope.ServiceProvider.GetRequiredService<SlagModeDBContext>();
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

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception ex)
{
    Debug.WriteLine(ex.Message);
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
           || request.Path.StartsWithSegments("/SlagMode");
}
