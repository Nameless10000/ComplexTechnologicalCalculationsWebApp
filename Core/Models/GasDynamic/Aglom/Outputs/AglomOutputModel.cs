using System.ComponentModel;

namespace Core.Models.GasDynamic.Aglom.Outputs;

public class AglomOutputModel : Entity
{
    [DisplayName("Порозность Агломерата")] public double AglomPorosity { get; set; }

    [DisplayName("Порозность Окатышей")] public double OkatPorosity { get; set; }
}