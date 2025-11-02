using System.ComponentModel;

namespace BaseLib.Models2.Aglom;

public class AglomContent : KoksContent
{
    [DisplayName("Порозность, м3/м3")]
    public double Porosity { get; set; }
}