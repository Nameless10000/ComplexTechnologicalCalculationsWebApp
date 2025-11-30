using System.Diagnostics;
using BaseLib.SlagMode.Models;
using Core.Contexts;
using Core.Models.Auth;
using Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
builder.Services.ScanServices();
builder.Services.ScanRepos();
builder.Services.ScanMathLibs();
builder.Services.ConfigMapper();

builder.Services.AddIdentity<User, Role>(options =>
    {
        // ����������� ����� ������
        options.Password.RequiredLength = 8;

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
    options.LoginPath = "/Account/Login";

    // ���� ��� ������ ������������ �� �������
    options.LogoutPath = "/Account/Logout";

    // ������ cookie ���������� ������ ��� HTTP-��������, ����� �� ������ ���� ��������� ����� JavaScript
    options.Cookie.HttpOnly = true;

    // ����� ����� cookie � ����� ����� ������� ������������ ������������� ������
    options.ExpireTimeSpan = TimeSpan.FromHours(1);

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

    agloDB.Database.Migrate();
    authDb.Database.Migrate();
    gasDb.Database.Migrate();
    matDb.Database.Migrate();
    slagDb.Database.Migrate();
    tbalDb.Database.Migrate();
    tmodeDb.Database.Migrate();

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

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