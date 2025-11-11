using System.ComponentModel;

namespace BaseLib.Models2.Base.Outputs;

public class TopGas
{
    [DisplayName("Объём СО2 при разложении известняка")]
    public double Volume_CO2_Izvest { get; set; }

    [DisplayName("Объём СО2 при косвенном восстановлении оксидов железа")]
    public double Volume_CO2_Kvost { get; set; }

    [DisplayName("Объём СО2 в колошниковом газе")]
    public double Volume_CO2_Kolgaz { get; set; }

    [DisplayName("Объём СО в колошниковом газе")]
    public double Volume_CO_Kolgaz { get; set; }

    [DisplayName("Объём СН4 в колошниковом газе")]
    public double Volume_CH4_Kolgaz { get; set; }

    [DisplayName("Объём N2 в колошниковом газе")]
    public double Volume_N2_Kolgaz { get; set; }

    [DisplayName("Объём Н2 в колошниковом газе")]
    public double Volume_H2_Kolgaz { get; set; }

    [DisplayName("Удельный выход колошникового газа при н.у.")]
    public double Udeln_Kolgaz { get; set; }

    [DisplayName("Расчётный состав колошникового газа CO2")]
    public double Kolgaz_CO2 { get; set; }

    [DisplayName("Расчётный состав колошникового газа CO")]
    public double Kolgaz_CO { get; set; }

    [DisplayName("Расчётный состав колошникового газа H2")]
    public double Kolgaz_H2 { get; set; }

    [DisplayName("Расчётный состав колошникового газа CH4")]
    public double Kolgaz_CH4 { get; set; }

    [DisplayName("Расчётный состав колошникового газа N2")]
    public double Kolgaz_N2 { get; set; }

    [DisplayName("Плотность колошникового газа при н.у. в верхней зоне")]
    public double Kolgaz_Plotn { get; set; }

    [DisplayName("Секундный выход колошникового газа")]
    public double Kolgaz_Minut { get; set; }
}