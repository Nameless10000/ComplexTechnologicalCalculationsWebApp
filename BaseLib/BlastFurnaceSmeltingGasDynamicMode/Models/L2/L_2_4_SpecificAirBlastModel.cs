namespace BaseLib.BlastFurnaceSmeltingGasDynamicMode.Models.L2;

/// <summary>
/// Л2.4 — Удельный расход дутья, м3/т чугуна
/// </summary>
public class L_2_4_SpecificAirBlastModel
{
    /// <summary>
    /// СФ — количество углерода кокса, сгорающего у фурм, кг/т.
    /// </summary>
    public double Cf { get; set; }

    /// <summary>
    /// VД — суммарный расход сухого дутья, м3/кг кокса (из Л2.3).
    /// </summary>
    public double Vd { get; set; }

    /// <summary>
    /// Результат: удельный расход дутья, м3/т чугуна.
    /// </summary>
    public double Qd => Vd * Cf;
}