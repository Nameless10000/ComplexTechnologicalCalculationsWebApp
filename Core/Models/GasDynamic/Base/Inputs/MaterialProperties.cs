using System.ComponentModel;

namespace Core.Models.GasDynamic.Base.Inputs;

public class MaterialProperties : Entity
{
    [DisplayName("Плотность шлака, кг/м³")]
    public double Plotn_shlak { get; set; }

    [DisplayName("Удельный выход шлака, кг/т чугуна")]
    public double Udeln_vyhod_shlak { get; set; }

    [DisplayName("Насыпная масса кокса, кг/м³")]
    public double Massa_koks_kg { get; set; }

    [DisplayName("Насыпная масса кокса, т/м³")]
    public double Massa_koks_t => Massa_koks_kg / 1e3;

    [DisplayName("Насыпная масса агломерата, т/м³")]
    public double Massa_aglo { get; set; }

    [DisplayName("Насыпная масса окатышей, т/м³")]
    public double Massa_okat { get; set; }

    [DisplayName("Порозность агломерата, м³/м³")]
    public double Porozn_aglo { get; set; }

    [DisplayName("Порозность окатышей, м³/м³")]
    public double Porozn_okat { get; set; }

    [DisplayName("Потеря массы при прокаливании, %")]
    public double Poteri_prokalivanie { get; set; }
}