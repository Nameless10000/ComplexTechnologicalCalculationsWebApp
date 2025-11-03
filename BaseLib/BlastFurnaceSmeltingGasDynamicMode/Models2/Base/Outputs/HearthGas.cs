using System.ComponentModel;

namespace BaseLib.Models2.Base.Outputs;

public class HearthGas
{
    [DisplayName("Выход фурменного газа на 1кг С кокса")]
    public double Furmgaz_Koks { get; set; }

    [DisplayName("Выход фурменного газа при конверсии 1м3 природного газа")]
    public double Furmgaz_Prir_Gaz { get; set; }

    [DisplayName("Суммарный выход фурменного газа")]
    public double Furmgaz_Sum { get; set; }

    [DisplayName("Удельный выход фурменного (горнового) газа")]
    public double Furmgaz_Udeln { get; set; }

    [DisplayName("Состав фурменного (горнового) газа СО")]
    public double Furmgaz_CO_V { get; set; }

    [DisplayName("Состав фурменного (горнового) газа Н2")]
    public double Furmgaz_H2_V { get; set; }

    [DisplayName("Состав фурменного (горнового) газа N2")]
    public double Furmgaz_N2_V { get; set; }

    [DisplayName("Содержание отдельных составляющих в горновом газе CO")]
    public double Furmgaz_CO { get; set; }

    [DisplayName("Содержание отдельных составляющих в горновом газе H2")]
    public double Furmgaz_H2 { get; set; }

    [DisplayName("Содержание отдельных составляющих в горновом газе N2")]
    public double Furmgaz_N2 { get; set; }

    [DisplayName("Теплосодержание горновых газов при теоретической температуре горения")]
    public double Teplosod_Furmgaz { get; set; }

    [DisplayName("Теоретическая температуре горения")]
    public double Temp_Teor { get; set; }

    [DisplayName("Средняя температура газов в нижней зоне печи")]
    public double Temp_Sredn_Niz { get; set; }
}