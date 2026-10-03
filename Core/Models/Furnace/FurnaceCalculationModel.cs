namespace Core.Models.Furnace;

public class FurnaceCalculationModel : Entity
{
    public string SerializedInput { get; set; }

    public string SerializedOutput { get; set; }

    public int OwnerId { get; set; }

    public bool IsPreset { get; set; } = false;
}