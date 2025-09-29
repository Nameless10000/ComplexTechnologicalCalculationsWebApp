namespace BaseLib.BlastFurnaceSmeltingGasDynamicMode.Models.L2;

/// <summary>
/// Л2.5 — Удельный расход кислорода в дутье, м3/т чугуна
/// </summary>
public class L_2_5_SpecificOxygenConsumptionModel
{
    /// <summary>
    /// Удельный расход дутья (из Л2.4), м3/т чугуна.
    /// </summary>
    public double Qd { get; set; }

    /// <summary>
    /// Содержание кислорода в дутье, %.
    /// </summary>
    public double O2 { get; set; }

    /// <summary>
    /// Результат: удельный расход кислорода в дутье, м3/т чугуна.
    /// </summary>
    public double Qd_O2 => Qd * O2 * 0.01;
}