using Confluent.Kafka;
using Contracts.Grpc;
using Core.Contexts;
using Data.Infrastructure;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace Web.Infrastructure;

public sealed class PostgreSqlHealthCheck(IServiceScopeFactory scopes) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken token = default)
    {
        using var scope = scopes.CreateScope();
        var types = new[] { typeof(AuthDBContext), typeof(AgloDBContext), typeof(SlagModeDBContext), typeof(GasDynamicDBContext), typeof(FurnaceDBContext), typeof(MatBalDBContext), typeof(TBalDBContext), typeof(TModeDBContext) };
        foreach (var type in types)
        {
            var db = (DbContext)scope.ServiceProvider.GetRequiredService(type);
            if (!await db.Database.CanConnectAsync(token)) return HealthCheckResult.Unhealthy($"База {type.Name} недоступна.");
        }
        return HealthCheckResult.Healthy("Все базы PostgreSQL доступны.");
    }
}

public sealed class KafkaHealthCheck(IOptions<KafkaOptions> options) : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken token = default) => Task.Run(() =>
    {
        try
        {
            using var client = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = options.Value.BootstrapServers, SocketTimeoutMs = 3000 }).Build();
            var metadata = client.GetMetadata(TimeSpan.FromSeconds(3));
            return metadata.Brokers.Count > 0 ? HealthCheckResult.Healthy("Kafka доступна.") : HealthCheckResult.Degraded("Kafka недоступна; события остаются в Outbox.");
        }
        catch { return HealthCheckResult.Degraded("Kafka недоступна; события остаются в Outbox."); }
    }, token);
}

public sealed class CalculationGrpcHealthCheck(IConfiguration configuration) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken token = default)
    {
        foreach (var module in new[] { "AglomMode", "SlagMode", "GasDynamic", "FurnaceService" })
        {
            try
            {
                using var channel = GrpcChannel.ForAddress(configuration[$"GrpcServices:{module}"]!);
                var deadline = DateTime.UtcNow.AddSeconds(3);
                HealthReply reply = module switch
                {
                    "AglomMode" => await new AglomCalculator.AglomCalculatorClient(channel).CheckHealthAsync(new HealthRequest(), deadline: deadline, cancellationToken: token),
                    "SlagMode" => await new SlagCalculator.SlagCalculatorClient(channel).CheckHealthAsync(new HealthRequest(), deadline: deadline, cancellationToken: token),
                    "GasDynamic" => await new GasDynamicCalculator.GasDynamicCalculatorClient(channel).CheckHealthAsync(new HealthRequest(), deadline: deadline, cancellationToken: token),
                    _ => await new Contracts.Grpc.FurnaceService.FurnaceServiceClient(channel).CheckHealthAsync(new HealthRequest(), deadline: deadline, cancellationToken: token)
                };
                if (reply.Status != "Serving") return HealthCheckResult.Unhealthy($"gRPC {module} не готов.");
            }
            catch (Exception) when (!token.IsCancellationRequested) { return HealthCheckResult.Unhealthy($"gRPC {module} недоступен."); }
        }
        return HealthCheckResult.Healthy("Все расчётные gRPC-сервисы доступны.");
    }
}
