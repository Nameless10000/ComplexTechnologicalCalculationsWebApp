namespace BaseLib.BlastFurnaceSmeltingGasDynamicMode.Models.L1;

/// <summary>
/// Л1.1. Количество пришедшего в печь углерода кокса, кг/т чугуна
/// </summary>
public class L_1_1_CarbonInputModel
{
    /// <summary>
    /// Содержание углерода в коксе, %
    /// </summary>
    public double Cnel { get; set; }

    /// <summary>
    /// Коэффициент использования кокса (расход кокса на 1 т чугуна), кг/т
    /// </summary>
    public double K { get; set; }

    /// <summary>
    /// Количество углерода кокса, пришедшего в печь, кг/т чугуна (СПРИШ)
    /// </summary>
    public double Cprish => 0.01 * Cnel * K;
}