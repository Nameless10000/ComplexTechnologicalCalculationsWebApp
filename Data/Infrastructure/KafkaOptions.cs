namespace Data.Infrastructure;

public sealed class KafkaOptions
{
    public string BootstrapServers { get; set; } = "localhost:9092";

    public string CalculationHistoryTopic { get; set; } = "calculation-history";
}
