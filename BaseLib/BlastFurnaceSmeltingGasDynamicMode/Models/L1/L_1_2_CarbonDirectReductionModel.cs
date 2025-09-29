namespace BaseLib.BlastFurnaceSmeltingGasDynamicMode.Models.L1;

/// <summary>
/// Л1.2. Расход углерода кокса на прямое восстановление оксидов Fe, Mn, P, Si и десульфурацию чугуна, кг/т чугуна
/// </summary>
public class L_1_2_CarbonDirectReductionModel
{
    /// <summary>
    /// Содержание железа в шихте, %
    /// </summary>
    public double FeContent { get; set; }

    /// <summary>
    /// Содержание марганца в шихте, %
    /// </summary>
    public double MnContent { get; set; }

    /// <summary>
    /// Содержание фосфора в шихте, %
    /// </summary>
    public double PContent { get; set; }

    /// <summary>
    /// Содержание кремния в шихте, %
    /// </summary>
    public double SiContent { get; set; }

    /// <summary>
    /// Содержание серы (сернистость чугуна), %
    /// </summary>
    public double SulfurContent { get; set; }

    /// <summary>
    /// Коэффициент Rd (берется из методики расчета)
    /// </summary>
    public double Rd { get; set; }

    public double Cpr => FeContent * 10 * Rd * (12d / 56d)
                         + 10 * MnContent * (12d / 55d)
                         + 10 * PContent * (60d / 62d)
                         + 10 * SiContent * (24d / 28d)
                         + 10 * SulfurContent * (12d / 32d);
}