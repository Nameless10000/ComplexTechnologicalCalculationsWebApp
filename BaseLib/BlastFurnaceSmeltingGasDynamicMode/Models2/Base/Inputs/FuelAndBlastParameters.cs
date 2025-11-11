using System.ComponentModel;

namespace BaseLib.Models2.Base.Inputs;

public class FuelAndBlastParameters
{
    [DisplayName("Удельный расход кокса, кг/т чугуна")]
    public double Udeln_koks { get; set; }

    [DisplayName("Содержание нелетучего углерода в коксе, %")]
    public double C_neletuch { get; set; }

    [DisplayName("Степень прямого восстановления (rd)")]
    public double Stepen_pryamogo_vost { get; set; }

    [DisplayName("Содержание кислорода в дутье, %")]
    public double Kislorod_dut { get; set; }

    [DisplayName("Влажность дутья, г/м³")]
    public double Vlazhn_dut { get; set; }

    [DisplayName("Удельный расход природного газа, м³/т чугуна")]
    public double Udeln_prir_gaz { get; set; }

    [DisplayName("Содержание C в природном газе (CH₄), м³/м³")]
    public double C_prir_gaz { get; set; }

    [DisplayName("Содержание H₂ в природном газе (2CH₄), м³/м³")]
    public double H2_prir_gaz { get; set; }

    [DisplayName("Степень использования водорода (nH₂)")]
    public double Stepen_vodorod { get; set; }

    [DisplayName("Степень использования CO (nCO)")]
    public double Stepen_CO { get; set; }

    [DisplayName("Минутный расход дутья, м³/мин")]
    public double Rashod_dut { get; set; }

    [DisplayName("Потери давления горячего дутья, %")]
    public double Poteri_dut { get; set; }
}