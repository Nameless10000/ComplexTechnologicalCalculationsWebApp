using SlagModeService.Services;
using BaseLib.SlagMode;
using BaseLib.SlagMode.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.Configure<ExternalServerDomain>(builder.Configuration.GetSection("ExternalServer"));
builder.Services.AddSingleton<SlagMode>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<SlagCalculationGrpcService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
