namespace Core.Models.Calculations;

public class OutboxMessage
{
    public Guid Id { get; set; }
    public Guid CalculationId { get; set; }
    public CalculationRecord Calculation { get; set; } = null!;
    public string Payload { get; set; } = "{}";
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public DateTime NextRetryAt { get; set; }
    public int Attempts { get; set; }
    public string? LastError { get; set; }
}
