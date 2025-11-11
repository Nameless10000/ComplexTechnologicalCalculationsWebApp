using System.ComponentModel;

namespace BaseLib.Models2.Base.Outputs;

public class BlastParameters
{
    [DisplayName("Расход сухого дутья на 1 кг С кокса")]
    public double Rashod_Dut_Koks { get; set; }

    [DisplayName("Расход сухого дутья для конверсии 1 м3 прир. газа.")]
    public double Rashod_Dut_Prir_Gaz { get; set; }

    [DisplayName("Суммарный расход сухого дутья")]
    public double Rashod_Dut_Sum { get; set; }

    [DisplayName("Расчетный удельный расход дутья")]
    public double Rashod_Dut_Udeln { get; set; }

    [DisplayName("Расчетное значение минутного расхода дутья")]
    public double Rashod_Dut_Minut { get; set; }

    [DisplayName("Скорость истечения дутья из фурмы")]
    public double Speed_Dut_Furm { get; set; }

    [DisplayName("Кинематическая вязкость дутья")]
    public double Vyazkost_Dut { get; set; }

    [DisplayName("Значение критерия Рейнольдса")]
    public double Reinolds { get; set; }

    [DisplayName("Критический расход дутья")]
    public double Rashod_Dut_Krit { get; set; }
}