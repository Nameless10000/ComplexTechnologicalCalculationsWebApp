namespace BaseLib.BlastFurnaceSmeltingGasDynamicMode.Models.L2;

/// <summary>
/// Л2.1 — Расход дутья для сжигания 1 кг углерода кокса, м3/кг кокса
/// </summary>
public class L_2_1_AirBlastPerKgCokeModel
{
    /// <summary>
    /// Параметр f (коэффициент из методики), безразмерный.
    /// </summary>
    public double F { get; set; }

    /// <summary>
    /// Параметр O2 (расход кислорода?)
    /// </summary>
    public double O2 { get; set; }

    /// <summary>
    /// Результат: расход дутья для сжигания 1 кг углерода кокса, м3/кг.
    /// </summary>
    public double Vd_ => 0.9333 / (0.01 * O2 + 0.00062 * F);
}