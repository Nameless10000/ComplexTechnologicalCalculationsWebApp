using AutoMapper;
using BaseLib.AglomMode.Models;
using BaseLib.Models2;
using BaseLib.SlagMode.Models;
using Confluent.Kafka;
using Contracts.History;
using Core.Contexts;
using Core.Models.AglomMode;
using Core.Models.GasDynamic;
using Core.Models.SlagMode;
using Data.Infrastructure;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CalculationHistoryWriter;

public class Worker(
    ILogger<Worker> logger,
    IServiceScopeFactory scopeFactory,
    IOptions<KafkaOptions> options)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var kafkaOptions = options.Value;
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = kafkaOptions.BootstrapServers,
            GroupId = "calculation-history-writer",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false
        };

        using var consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
        consumer.Subscribe(kafkaOptions.CalculationHistoryTopic);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var result = consumer.Consume(stoppingToken);
                var historyEvent = JsonConvert.DeserializeObject<CalculationHistoryEvent>(result.Message.Value);

                if (historyEvent is null)
                {
                    logger.LogWarning("Skipped empty calculation history event.");
                    consumer.Commit(result);
                    continue;
                }

                await SaveHistoryAsync(historyEvent, stoppingToken);
                consumer.Commit(result);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to process calculation history event.");
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }

        consumer.Close();
    }

    private async Task SaveHistoryAsync(CalculationHistoryEvent historyEvent, CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

        switch (historyEvent.Module)
        {
            case CalculationModules.AglomMode:
                await SaveAglomModeAsync(scope.ServiceProvider, mapper, historyEvent, cancellationToken);
                break;
            case CalculationModules.GasDynamic:
                await SaveGasDynamicAsync(scope.ServiceProvider, historyEvent, cancellationToken);
                break;
            case CalculationModules.SlagMode:
                await SaveSlagModeAsync(scope.ServiceProvider, mapper, historyEvent, cancellationToken);
                break;
            default:
                logger.LogWarning("Unknown calculation module '{Module}'.", historyEvent.Module);
                break;
        }
    }

    private static async Task SaveAglomModeAsync(
        IServiceProvider serviceProvider,
        IMapper mapper,
        CalculationHistoryEvent historyEvent,
        CancellationToken cancellationToken)
    {
        var dbContext = serviceProvider.GetRequiredService<AgloDBContext>();
        var requestModel = JsonConvert.DeserializeObject<AglomRequestData>(historyEvent.RequestJson)!;
        var responseModel = JsonConvert.DeserializeObject<AglomResponseData>(historyEvent.ResponseJson)!;

        var request = mapper.Map<AglomRequestData, AglomRequestDB>(requestModel);
        var response = mapper.Map<AglomResponseData, AglomResponseDB>(responseModel);

        request.AglomResponse = response;
        request.CreatorID = historyEvent.UserId;
        request.CreationDateTime = historyEvent.CreationDateTime;
        response.CreatorID = historyEvent.UserId;
        response.CreationDateTime = historyEvent.CreationDateTime;

        await dbContext.AglomRequests.AddAsync(request, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static async Task SaveGasDynamicAsync(
        IServiceProvider serviceProvider,
        CalculationHistoryEvent historyEvent,
        CancellationToken cancellationToken)
    {
        var dbContext = serviceProvider.GetRequiredService<GasDynamicDBContext>();
        var calculation = new CalculationModel
        {
            OwnerId = historyEvent.UserId,
            CreatorID = historyEvent.UserId,
            CreationDateTime = historyEvent.CreationDateTime,
            SerializedInput = historyEvent.RequestJson,
            SerializedOutput = historyEvent.ResponseJson
        };

        await dbContext.CalculationModels.AddAsync(calculation, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static async Task SaveSlagModeAsync(
        IServiceProvider serviceProvider,
        IMapper mapper,
        CalculationHistoryEvent historyEvent,
        CancellationToken cancellationToken)
    {
        var dbContext = serviceProvider.GetRequiredService<SlagModeDBContext>();
        var requestModel = JsonConvert.DeserializeObject<RequestData>(historyEvent.RequestJson)!;
        var responseModel = JsonConvert.DeserializeObject<ResponseData>(historyEvent.ResponseJson)!;

        var request = mapper.Map<RequestData, Request>(requestModel);
        var response = mapper.Map<ResponseData, Response>(responseModel);

        response.Request = request;
        response.CreatorID = historyEvent.UserId;
        response.CreationDateTime = historyEvent.CreationDateTime;
        request.CreatorID = historyEvent.UserId;
        request.CreationDateTime = historyEvent.CreationDateTime;

        await dbContext.Responses.AddAsync(response, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
