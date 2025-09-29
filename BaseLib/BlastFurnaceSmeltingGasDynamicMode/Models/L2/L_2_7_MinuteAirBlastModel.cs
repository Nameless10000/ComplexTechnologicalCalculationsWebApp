namespace BaseLib.BlastFurnaceSmeltingGasDynamicMode.Models.L2;

/// <summary>
/// Л2.7 — Минутный расход дутья, м3/мин
/// </summary>
public class L_2_7_MinuteAirBlastModel
{
    /// <summary>
    /// Удельный расход дутья (из Л2.4), м3/т чугуна.
    /// </summary>
    public double Qd { get; set; }

    /// <summary>
    /// Производительность доменной печи (PД), т чугуна/сутки.
    /// </summary>
    public double P { get; set; }

    /// <summary>
    /// Результат: минутный расход дутья, м3/мин.
    /// </summary>
    public double QdP => Qd * P / 1440;
}