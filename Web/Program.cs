using Core.Contexts;
using Core.Services;
using Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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

builder.Services.ConfigureDataBaseContexts(conStrings);
builder.Services.ScanServices();
builder.Services.ScanRepos();
builder.Services.ConfigMapper();

builder.Services.AddTransient<SimpleLoggerService>();

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

    if (app.Environment.IsDevelopment())
    {
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
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch(Exception ex)
{
    Debug.WriteLine(ex.Message);
}