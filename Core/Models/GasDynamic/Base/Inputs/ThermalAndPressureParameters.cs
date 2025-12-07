using System.ComponentModel;

namespace Core.Models.GasDynamic.Base.Inputs;

public class ThermalAndPressureParameters : Entity
{
    [DisplayName("Температура горячего дутья, °C")]
    public double Temp_dut { get; set; }

    [DisplayName("Теплоёмкость кокса, кДж/(кг·К)")]
    public double Teploemk_koks { get; set; }

    [DisplayName("Температура кокса у фурм, °C")]
    public double Temp_koks { get; set; }

    [DisplayName("Теплота неполного горения С кокса, кДж/кг")]
    public double Teplota_nepoln_koks { get; set; }

    [DisplayName("Теплота неполного горения природного газа, кДж/м³")]
    public double Teplota_nepoln_prir_gaz { get; set; }

    [DisplayName("Избыточное давление горячего дутья, ати")]
    public double Davlen_izb_dut { get; set; }

    [DisplayName("Избыточное давление колошникового газа, ати")]
    public double Davlen_izb_koloshnik_gaz { get; set; }

    [DisplayName("Температура колошникового газа, °C")]
    public double Temp_koloshnik_gaz { get; set; }

    [DisplayName("Нижний перепад давления (измеренный), атм")]
    public double Perepad_niz { get; set; }

    [DisplayName("Верхний перепад давления (измеренный), атм")]
    public double Perepad_verh { get; set; }
}