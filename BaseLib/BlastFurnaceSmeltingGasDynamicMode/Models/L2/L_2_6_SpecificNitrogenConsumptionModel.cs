namespace BaseLib.BlastFurnaceSmeltingGasDynamicMode.Models.L2;

/// <summary>
/// Л2.6 — Удельный расход азота в дутье, м3/т чугуна
/// </summary>
public class L_2_6_SpecificNitrogenConsumptionModel
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
    /// Результат: удельный расход азота в дутье, м3/т чугуна.
    /// </summary>
    public double Qd_N2 => Qd * (100 - O2) * 0.01;
}