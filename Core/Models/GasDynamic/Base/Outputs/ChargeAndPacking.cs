using System.ComponentModel;

namespace Core.Models.GasDynamic.Base.Outputs;

public class ChargeAndPacking : Entity
{
    [DisplayName("Объёмы шихтовых материалов на 1 тонну чугуна — агломерат")]
    public double Volume_Aglo_1chugun { get; set; }

    [DisplayName("Объёмы шихтовых материалов на 1 тонну чугуна — окатыши")]
    public double Volume_Okat_1chugun { get; set; }

    [DisplayName("Объёмы шихтовых материалов на 1 тонну чугуна — кокс")]
    public double Volume_Koks_1chugun { get; set; }

    [DisplayName("Суммарный объём шихты на получение 1 тонны чугуна")]
    public double Volume_Sum_1chugun { get; set; }

    [DisplayName("Насыпная масса слоя шихты")]
    public double Massa_Nasyp_Shihta { get; set; }

    [DisplayName("Объёмные доли компонентов шихты — агломерат")]
    public double Shihta_Dolya_Aglo { get; set; }

    [DisplayName("Объёмные доли компонентов шихты — окатыши")]
    public double Shihta_Dolya_Okat { get; set; }

    [DisplayName("Объёмные доли компонентов шихты — кокс")]
    public double Shihta_Dolya_Koks { get; set; }

    [DisplayName("Порозность слоя шихты в печи в верхней зоне")]
    public double Shihta_Porozn_Verh { get; set; }

    [DisplayName("Эквивалентный диаметр куска шихты в верхней зоне")]
    public double Shihta_Diam_Verh { get; set; }

    [DisplayName("Эквивалентный диаметр кусков кокса")]
    public double Diam_Koks { get; set; }

    [DisplayName("Порозность слоя кокса")] public double Porozn_Koks { get; set; }

    [DisplayName("Удельный объём образующегося шлака")]
    public double Volume_Udeln_Shlak { get; set; }

    [DisplayName("Удельный объём кокса на получение 1 т чугуна")]
    public double Volume_Udeln_Koks { get; set; }

    [DisplayName("Удельный объём межкусковых пространств коксовой насадки")]
    public double Volume_Udeln_Nasadzka { get; set; }

    [DisplayName("Оставшийся удельный объём межкусковых пространств коксовой насадки")]
    public double Volume_Udeln_Ost { get; set; }

    [DisplayName("Скоректированная порозность слоя (с учетом нахождения в насадке шлак раслава)")]
    public double Porozn_Sloy_Korrekt { get; set; }

    [DisplayName("Эквивалентный диаметр куска агломерата")]
    public double Diam_Aglo { get; set; }

    [DisplayName("Порозность слоя агломерата")]
    public double Porozn_Aglo { get; set; }

    [DisplayName("Порозность слоя окатышей")]
    public double Porozn_Okat { get; set; }
}