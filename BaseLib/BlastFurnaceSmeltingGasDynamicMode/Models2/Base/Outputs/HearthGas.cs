using System.ComponentModel;

namespace BaseLib.Models2.Base.Outputs;

public class HearthGas
{
    [DisplayName("Выход фурменного газа на 1кг С кокса")] public double FurmgazKoks { get; set; }
    [DisplayName("Выход фурменного газа при конверсии 1м3 природного газа")] public double FurmgazPrirGaz { get; set; }
    [DisplayName("Суммарный выход фурменного газа")] public double FurmgazSum { get; set; }
    [DisplayName("Удельный выход фурменного (горнового) газа")] public double FurmgazUdeln { get; set; }

    [DisplayName("Состав фурменного (горнового) газа СО")] public double FurmgazCOV { get; set; }
    [DisplayName("Состав фурменного (горнового) газа Н2")] public double FurmgazH2V { get; set; }
    [DisplayName("Состав фурменного (горнового) газа N2")] public double FurmgazN2V { get; set; }

    [DisplayName("Содержание отдельных составляющих в горновом газе CO")] public double FurmgazCO { get; set; }
    [DisplayName("Содержание отдельных составляющих в горновом газе H2")] public double FurmgazH2 { get; set; }
    [DisplayName("Содержание отдельных составляющих в горновом газе N2")] public double FurmgazN2 { get; set; }

    [DisplayName("Теплосодержание горновых газов при теоретической температуре горения")] public double TeplosodFurmgaz { get; set; }
    [DisplayName("Теоретическая температуре горения")] public double TempTeor { get; set; }
    [DisplayName("Средняя температура газов в нижней зоне печи")] public double TempSrednNiz { get; set; }
}