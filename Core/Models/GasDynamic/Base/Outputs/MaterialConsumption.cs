using System.ComponentModel;

namespace Core.Models.GasDynamic.Base.Outputs;

public class MaterialConsumption : Entity
{
    [DisplayName("Удельный расход агломерата")]
    public double Udeln_Aglo { get; set; }

    [DisplayName("Удельный расход окатышей")]
    public double Udeln_Okat { get; set; }

    [DisplayName("Удельный расход известняка")]
    public double Udeln_Izvest { get; set; }

    [DisplayName("Удельный расход кокса, выраженный в т/т чугуна")]
    public double Udeln_Koks_1000 { get; set; }

    [DisplayName("Масса шихты на 1 тонну чугуна")]
    public double Udeln_Sum { get; set; }

    [DisplayName("Расход природного газа в расчёте на 1кг кокса")]
    public double Rashod_Prir_Gaz { get; set; }
}