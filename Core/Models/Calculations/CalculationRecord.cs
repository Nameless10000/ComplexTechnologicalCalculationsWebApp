namespace Core.Models.Calculations;

public class CalculationRecord
{
    public Guid Id { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; } = "";
    public string Module { get; set; } = "";
    public string RequestId { get; set; } = "";
    public string CorrelationId { get; set; } = "";
    public string RequestJson { get; set; } = "{}";
    public string ResponseJson { get; set; } = "{}";
    public string Status { get; set; } = "Pending";
    public Guid? SourceCalculationId { get; set; }
    public DateTime CreatedAt { get; set; }
}
