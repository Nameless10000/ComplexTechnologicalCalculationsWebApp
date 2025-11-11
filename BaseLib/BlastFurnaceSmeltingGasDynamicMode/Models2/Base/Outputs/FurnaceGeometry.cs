using System.ComponentModel;

namespace BaseLib.Models2.Base.Outputs;

public class FurnaceGeometry
{
    [DisplayName("Среднее значение поперечного сечения нижней зоны печи")]
    public double S_Sech_Niz { get; set; }

    [DisplayName("Средний диаметр нижней части доменной печи")]
    public double Diam_Niz { get; set; }

    [DisplayName("Средний диаметр верхней части печи")]
    public double Diam_Verh { get; set; }

    [DisplayName("Высота слоя шихты в нижней зоне печи")]
    public double Height_Shihta_Niz { get; set; }

    [DisplayName("Высота слоя шихты в верхней зоне печи")]
    public double Shihta_Height_Verh { get; set; }

    [DisplayName("Активная высота слоя шихты продуваемая газами")]
    public double Height_Aktiv { get; set; }
}