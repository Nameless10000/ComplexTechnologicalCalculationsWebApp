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
builder.Services.ConfigMapper();

builder.Services.AddIdentity<User, Role>(options =>
{
    // Минимальная длина пароля
    options.Password.RequiredLength = 8;

    // Нужно ли, чтобы пароль содержал хотя бы один неалфавитно-цифровой символ (например, !, @, #)
    options.Password.RequireNonAlphanumeric = false;

    // Нужно ли, чтобы пароль содержал хотя бы одну заглавную букву
    options.Password.RequireUppercase = false;

    // Можно добавить другие настройки:
    // options.Password.RequireDigit = true;       // Требовать цифру
    // options.Password.RequireLowercase = true;   // Требовать хотя бы одну строчную букву
    // options.User.RequireUniqueEmail = true;    // Требовать уникальный email при регистрации
})
.AddEntityFrameworkStores<AuthDBContext>() // Указывает, что Identity будет использовать AuthDBContext для хранения пользователей и ролей
.AddDefaultTokenProviders();               // Добавляет провайдеры токенов для подтверждения email, сброса пароля и т.д.

builder.Services.ConfigureApplicationCookie(options =>
{
    // Путь, на который перенаправляется пользователь, если он не авторизован
    options.LoginPath = "/Account/Login";

    // Путь для выхода пользователя из системы
    options.LogoutPath = "/Account/Logout";

    // Делает cookie доступными только для HTTP-запросов, чтобы их нельзя было прочитать через JavaScript
    options.Cookie.HttpOnly = true;

    // Время жизни cookie — после этого времени пользователь автоматически выйдет
    options.ExpireTimeSpan = TimeSpan.FromHours(1);

    // Можно добавить другие настройки:
    // options.Cookie.Name = "MyAppAuthCookie";  // Имя cookie
    // options.SlidingExpiration = true;         // Обновлять срок действия cookie при активности пользователя
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

    /*if (app.Environment.IsDevelopment())
    {

    }*/

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