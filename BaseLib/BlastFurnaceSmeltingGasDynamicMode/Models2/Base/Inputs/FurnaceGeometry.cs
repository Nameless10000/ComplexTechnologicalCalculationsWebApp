using System.ComponentModel;

namespace BaseLib.Models2.Base.Inputs;

public class FurnaceGeometry
{
    [DisplayName("Диаметр горна, м")]
    public double Diam_gorn { get; set; }

    [DisplayName("Диаметр распара, м")]
    public double Diam_raspar { get; set; }

    [DisplayName("Диаметр колошника, м")]
    public double Diam_koloshnik { get; set; }

    [DisplayName("Высота заплечиков, м")]
    public double Height_zaplechik { get; set; }

    [DisplayName("Высота распара, м")]
    public double Height_raspar { get; set; }

    [DisplayName("Высота шахты, м")]
    public double Height_shahta { get; set; }

    [DisplayName("Высота колошника, м")]
    public double Height_koloshnik { get; set; }

    [DisplayName("Уровень засыпи, м")]
    public double Uroven_zasypi { get; set; }

    [DisplayName("Число работающих воздушных фурм, шт")]
    public double Kolvo_furm { get; set; }

    [DisplayName("Диаметр воздушных фурм, мм")]
    public double Diam_furm { get; set; }

    [DisplayName("Длина фурмы, мм")]
    public double Dlina_furm { get; set; }
}
