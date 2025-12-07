using System.ComponentModel;

namespace Core.Models.GasDynamic.Base.Inputs;

public class ProductionParameters : Entity
{
    [DisplayName("Суточная производительность по чугуну, т/сут")]
    public double Proizvodit_chugun { get; set; }

    [DisplayName("Удельный расход железорудного материала, т/т чугуна")]
    public double Udeln_zhelezorud { get; set; }

    [DisplayName("Удельный расход известняка, кг/т чугуна")]
    public double Udeln_izvest { get; set; }

    [DisplayName("Критическая степень уравновешивания шихты, %")]
    public double Stepen_urav_krit { get; set; }

    [DisplayName("Доля окатышей в шихте, %")]
    public double Dolya_okat { get; set; }
}