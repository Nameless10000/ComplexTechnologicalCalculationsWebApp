using Confluent.Kafka;
using Contracts.History;
using Data.Infrastructure;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Data.Services;

public sealed class CalculationHistoryProducerService : IDisposable
{
    private readonly IProducer<string, string> _producer;
    private readonly KafkaOptions _options;

    public CalculationHistoryProducerService(IOptions<KafkaOptions> options)
    {
        _options = options.Value;
        _producer = new ProducerBuilder<string, string>(new ProducerConfig
        {
            BootstrapServers = _options.BootstrapServers
        }).Build();
    }

    public async Task PublishAsync(CalculationHistoryEvent historyEvent, CancellationToken cancellationToken = default)
    {
        await _producer.ProduceAsync(
            _options.CalculationHistoryTopic,
            new Message<string, string>
            {
                Key = $"{historyEvent.Module}:{historyEvent.UserId}",
                Value = JsonConvert.SerializeObject(historyEvent)
            },
            cancellationToken);
    }

    public void Dispose()
    {
        _producer.Flush(TimeSpan.FromSeconds(5));
        _producer.Dispose();
    }
}
