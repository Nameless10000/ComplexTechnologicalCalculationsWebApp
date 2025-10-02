using Core.Contexts;
using Data;

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();