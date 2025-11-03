using System.ComponentModel;

namespace BaseLib.Models2.Base.Inputs;

public class CompositionParameters
{
    [DisplayName("Содержание железа [Fe], %")]
    public double Fe_chugun { get; set; }

    [DisplayName("Содержание марганца [Mn], %")]
    public double Mn_chugun { get; set; }

    [DisplayName("Содержание фосфора [P], %")]
    public double P_chugun { get; set; }

    [DisplayName("Содержание кремния [Si], %")]
    public double Si_chugun { get; set; }

    [DisplayName("Содержание серы (S), %")]
    public double S_shlak { get; set; }

    [DisplayName("Содержание углерода [C], %")]
    public double C_chugun { get; set; }
}
