namespace BaseLib.BlastFurnaceSmeltingGasDynamicMode.Models.L2;

/// <summary>
/// Л2.3 — Суммарный расход сухого дутья, м3/кг кокса
/// </summary>
public class L_2_3_TotalAirBlastModel
{
    /// <summary>
    /// Расход дутья для сжигания углерода кокса (Л2.1), м3/кг.
    /// </summary>
    public double Vd_ { get; set; }

    /// <summary>
    /// Расход дутья для сжигания природного газа (Л2.2), м3/м3.
    /// </summary>
    public double Vd__ { get; set; }

    /// <summary>
    /// Вход: расход природного газа (VПГ), м3/т чугуна.
    /// </summary>
    public double Vpg { get; set; }

    public double Cf { get; set; }

    /// <summary>
    /// Результат: суммарный расход сухого дутья, м3/кг кокса.
    /// </summary>
    public double Vd => Vd_ + (Vpg / Cf) * Vd__;
}