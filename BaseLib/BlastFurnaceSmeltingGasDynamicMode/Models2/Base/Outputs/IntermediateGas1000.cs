using System.ComponentModel;

namespace BaseLib.Models2.Base.Outputs;

public class IntermediateGas1000
{
    [DisplayName("CO2, образующийся при восстановлении оксидов Fe, Mn, Si, P, а также при десульфурации")]
    public double Volume_CO_Pvost { get; set; }

    [DisplayName("Объём Vсо при t=1000")]
    public double Volume_CO_1000 { get; set; }

    [DisplayName("Объём Н2 при t=1000")]
    public double Volume_H2_1000 { get; set; }

    [DisplayName("Объём N2 при t=1000")]
    public double Volume_N2_1000 { get; set; }

    [DisplayName("Суммарный объём на температурной зоне t=1000")]
    public double Volume_Sum_1000 { get; set; }

    [DisplayName("Состав доменного газа CO на горизонте t=1000")]
    public double Domengaz_CO_1000 { get; set; }

    [DisplayName("Состав доменного газа H2 на горизонте t=1000")]
    public double Domengaz_H2_1000 { get; set; }

    [DisplayName("Состав доменного газа N2 на горизонте t=1000")]
    public double Domengaz_N2_1000 { get; set; }

    [DisplayName("Плотность газа на горизонте 1000 °С при н.у.")]
    public double Domengaz_Plotn_1000 { get; set; }

    [DisplayName("Секундный расход доменного газа на горизонте 1000 °С")]
    public double Domengaz_Rashod_1000 { get; set; }
}