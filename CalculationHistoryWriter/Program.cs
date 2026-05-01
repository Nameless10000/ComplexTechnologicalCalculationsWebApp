using CalculationHistoryWriter;

using Core.Contexts;
using Data;

var builder = Host.CreateApplicationBuilder(args);

var conStrings = new Dictionary<Type, string>
{
    [typeof(AgloDBContext)] = builder.Configuration.GetConnectionString("AgloConnectionString")!,
    [typeof(AuthDBContext)] = builder.Configuration.GetConnectionString("AuthConnectionString")!,
    [typeof(GasDynamicDBContext)] = builder.Configuration.GetConnectionString("GasDynamicConnectionString")!,
    [typeof(MatBalDBContext)] = builder.Configuration.GetConnectionString("MatBalConnectionString")!,
    [typeof(SlagModeDBContext)] = builder.Configuration.GetConnectionString("SlagModeConnectionString")!,
    [typeof(TBalDBContext)] = builder.Configuration.GetConnectionString("TBalConnectionString")!,
    [typeof(TModeDBContext)] = builder.Configuration.GetConnectionString("TModeConnectionString")!
};

builder.Services.ConfigureDataBaseContexts(conStrings);
builder.Services.ConfigureKafka(builder.Configuration);
builder.Services.ConfigMapper();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
