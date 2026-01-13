namespace Core.Models.GasDynamic;

public class CalculationModel : Entity
{
    public string SerializedInput { get; set; }

    public string SerializedOutput { get; set; }

    public int OwnerId { get; set; }

    public bool IsPreset { get; set; } = false;
}