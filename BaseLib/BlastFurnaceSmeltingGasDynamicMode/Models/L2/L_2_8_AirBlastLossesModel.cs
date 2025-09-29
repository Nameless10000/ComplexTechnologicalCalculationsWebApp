namespace BaseLib.BlastFurnaceSmeltingGasDynamicMode.Models.L2;

/// <summary>
/// Л2.8 — Потери дутья по тракту подачи, %
/// </summary>
public class L_2_8_AirBlastLossesModel
{
    /// <summary>
    /// Измеренный расход дутья по КИП, м3/мин.
    /// </summary>
    public double VdKIP { get; set; }

    /// <summary>
    /// Расчётный расход дутья, м3/мин.
    /// </summary>
    public double QdP { get; set; }

    /// <summary>
    /// Результат: потери дутья по тракту подачи, %.
    /// </summary>
    public double AirBlastLossesPercent =>
        (QdP - VdKIP) / QdP * 100;
}