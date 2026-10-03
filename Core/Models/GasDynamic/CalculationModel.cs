using Microsoft.EntityFrameworkCore;
namespace Core.Models.GasDynamic;

[Index(nameof(HistoryEventId), IsUnique = true)]
public class CalculationModel : Entity
{
    public Guid? HistoryEventId { get; set; }

    public string SerializedInput { get; set; }

    public string SerializedOutput { get; set; }

    public int OwnerId { get; set; }

    public bool IsPreset { get; set; } = false;
}