using System.ComponentModel;

namespace BaseLib.Models2.Aglom;

public class KoksContent
{
    [DisplayName("Минимальный размер фракции, мм")]
    public double MinFractionSize { get; set; }
    
    [DisplayName("Содержание фракции, %")]
    public double FractionPercentage { get; set; }

    [DisplayName("Доля фракции")]
    public double FractionPart => FractionPercentage / 100;
}