using System.ComponentModel;

namespace BaseLib.Models;

public class ResponseData
{
    [DisplayName("Степень уравновешивания шихты газовым потоком, %")]
    public double SU { get; set; }

    [DisplayName("Критический перепад давления газов, атм")]
    public double DeltaPkp { get; set; }

    [DisplayName("Критический расход дутья, м3/мин")]
    public double VdKP { get; set; }

    [DisplayName("Скорость фильтрации газового потока через верхнюю часть горна, м/с")]
    public double Wgorn { get; set; }

    [DisplayName("Действительная скорость газа через верхнюю часть горна, м/с")]
    public double WgornFact { get; set; }

    [DisplayName("Скорость фильтрации газового потока в области распара, м/с")]
    public double Wrasp { get; set; }

    [DisplayName("Действительная скорость газа в области распара, м/с")]
    public double WraspFact { get; set; }

    [DisplayName("Скорость фильтрации газового потока через колошник, м/с")]
    public double Wkolosh { get; set; }

    [DisplayName("Действительная скорость газа через колошник, м/с")]
    public double WkoloshFact { get; set; }
}