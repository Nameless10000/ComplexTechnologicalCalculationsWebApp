using Microsoft.EntityFrameworkCore;
namespace Core.Models.Furnace;

[Index(nameof(HistoryEventId), IsUnique = true)]
public class FurnaceCalculationModel : Entity
{
    public Guid? HistoryEventId { get; set; }

    public string SerializedInput { get; set; }

    public string SerializedOutput { get; set; }

    public int OwnerId { get; set; }

    public bool IsPreset { get; set; } = false;
}