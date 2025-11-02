using System.ComponentModel;

namespace BaseLib.Models2.Base.Outputs;

public class HydrodynamicsLower
{
    [DisplayName("Скорость фильтрации газового потока в нижней зоне печи при н.у.")]
    public double Speed_Filtr_Niz { get; set; }

    [DisplayName("Значение коэффициента трения")]
    public double Tren_Koef { get; set; }

    [DisplayName("Значение коэффициента трения с учетом потерь на тракте")]
    public double Tren_Sum { get; set; }

    [DisplayName("Потери напора дутья на воздушных фурмах")]
    public double Poteri_Furm { get; set; }

    [DisplayName("Нижний перепад давления газов по высоте слоя шихты")]
    public double Perepad_Niz_Itog { get; set; }

    [DisplayName("Доля нижнего перепада давления газов от общего перепада")]
    public double Perepad_Niz_Dolya { get; set; }

    [DisplayName("Избыточное давление газа на горизонте 1000 °С")]
    public double Davlen_Izb_1000 { get; set; }

    [DisplayName("Среднее значение абсолютного давления газа в нижней зоне печи")]
    public double Davlen_Niz { get; set; }

    [DisplayName("Коэффициент сопротивления")]
    public double Koef_Soprot_Niz { get; set; }

    [DisplayName("Коэффициент пропорциональности между минутным расходом дутья и скоростью фильтрации газа в нижней зоне печи")]
    public double Koef_Proporc_Dut_Filtr_Niz { get; set; }

    [DisplayName("Коэффициент АН")]
    public double Koef_An { get; set; }
}