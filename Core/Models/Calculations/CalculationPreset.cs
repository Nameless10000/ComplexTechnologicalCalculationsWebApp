namespace Core.Models.Calculations;

public class CalculationPreset
{
    public Guid Id { get; set; }
    public int UserId { get; set; }
    public string Module { get; set; } = "";
    public string Name { get; set; } = "";
    public string NormalizedName { get; set; } = "";
    public string Description { get; set; } = "";
    public string Payload { get; set; } = "{}";
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
