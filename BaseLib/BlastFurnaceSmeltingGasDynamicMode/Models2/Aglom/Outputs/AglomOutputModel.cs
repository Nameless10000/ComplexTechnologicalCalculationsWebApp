using System.ComponentModel;

namespace BaseLib.Models2.Aglom.Outputs;

public class AglomOutputModel
{
    [DisplayName("Порозность Агломерата")] public double AglomPorosity { get; set; }

    [DisplayName("Порозность Окатышей")] public double OkatPorosity { get; set; }
}