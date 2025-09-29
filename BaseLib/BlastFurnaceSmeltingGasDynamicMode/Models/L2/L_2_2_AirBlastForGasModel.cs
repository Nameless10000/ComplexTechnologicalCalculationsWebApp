namespace BaseLib.BlastFurnaceSmeltingGasDynamicMode.Models.L2;

/// <summary>
/// Л2.2 — Расход дутья для сжигания природного газа в фурменном очаге, м3/м3
/// </summary>
public class L_2_2_AirBlastForGasModel
{
    /// <summary>
    /// Параметр f (коэффициент), безразмерный.
    /// </summary>
    public double F { get; set; }

    /// <summary>
    /// Объем природного газа, подаваемый в очаг (VПГ), м3/т или м3/м3 (уточнить).
    /// </summary>
    public double O2 { get; set; }

    /// <summary>
    /// Результат: расход дутья для сжигания природного газа в очаге, м3/м3.
    /// </summary>
    public double Vd__ => 0.5 / (0.01 * O2 + 0.00063 * F);
}