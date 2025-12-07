using System.ComponentModel;

namespace Core.Models.GasDynamic.Base.Outputs;

public class ThermalParameters : Entity
{
    [DisplayName("Теплоёмкость двуатомных газов при температуре горячего дутья")]
    public double Teploemk_2atom { get; set; }

    [DisplayName("Теплоёмкость паров воды при температуре горячего дутья")]
    public double Teploemk_Voda { get; set; }

    [DisplayName("Теплосодержание горячего дутья за вычетом теплоты разложения влаги дутья")]
    public double Teplosod_Dut { get; set; }

    [DisplayName("Теплосодержание углеродо кокса, пришедшего к фурмам")]
    public double Teplosod_Koks { get; set; }

    [DisplayName("Средняя температура в верхней зоне")]
    public double Temp_Verh { get; set; }
}