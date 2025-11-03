using System.ComponentModel;

namespace BaseLib.Models2.Base.Outputs;

public class CarbonBalance
{
    [DisplayName("Количество углерода (С), пришедшего в печь с коксом")]
    public double C_Input { get; set; }

    [DisplayName("Расход С на прямое восстановление оксидов Fe, Mn,Si, а также на десульфурацию")]
    public double C_Out_Vosstan { get; set; }

    [DisplayName("Растворяется углерода в чугуне")]
    public double C_Out_Chugun { get; set; }

    [DisplayName("Расход С на образование метана")]
    public double C_Out_Metan { get; set; }

    [DisplayName("Количество С сгорающего у фурм")]
    public double C_Out_Furm { get; set; }
}