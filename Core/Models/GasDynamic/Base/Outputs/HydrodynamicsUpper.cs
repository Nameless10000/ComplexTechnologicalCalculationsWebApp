using System.ComponentModel;

namespace Core.Models.GasDynamic.Base.Outputs;

public class HydrodynamicsUpper : Entity
{
    [DisplayName("Скорость фильтрации газового потока в верхней зоне печи")]
    public double Speed_Filtr_Verh { get; set; }

    [DisplayName("Скорость фильтрации газового потока через верхнюю часть горна при н.у.")]
    public double Speed_Filtr_Gorn { get; set; }

    [DisplayName("Действительная скорость движения газа в верхней части горна печи")]
    public double Speed_Real_Verh { get; set; }

    [DisplayName("Скорость фильтрации газового потока в области распара печи")]
    public double Speed_Filtr_Raspar { get; set; }

    [DisplayName(
        "Скорость движения газа в области распара с учётом действительных параметров газа и с учётом порозности слоя")]
    public double Speed_Real_Raspar { get; set; }

    [DisplayName("Скорость фильтрации газового потока через колошник печи")]
    public double Speed_Filtr_Koloshnik { get; set; }

    [DisplayName(
        "Скорость движения газа через колошник с учётом действительных параметров газа и с учётом порозности слоя")]
    public double Speed_Real_Koloshnik { get; set; }

    [DisplayName("Абсолютное среднее давление в верхней зоне")]
    public double Davlen_Verh { get; set; }

    [DisplayName("Коэффициент сопротивления слоя шихты")]
    public double Koef_Soprot_Verh { get; set; }

    [DisplayName(
        "Коэффициент пропорциональности между минутным расходом дутья и скоростью фильтрации газа в верхней зоне печи")]
    public double Koef_Proporc_Dut_Filtr_Verh { get; set; }

    [DisplayName("Коэффициент АВ")] public double Koef_Av { get; set; }

    [DisplayName("Перепад давления газов по высоте слоя шихты")]
    public double Perepad_Davlen { get; set; }

    [DisplayName("Степень уравновешивания шихты")]
    public double Stepen_Urav { get; set; }
}