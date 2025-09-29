namespace BaseLib.BlastFurnaceSmeltingGasDynamicMode.Models.L1;

/// <summary>
/// Л1.3 — Переход углерода кокса в чугун (СЧ), кг/т чугуна
/// </summary>
public class L_1_3_CokeToPigIronTransferModel
{
    /// <summary>
    /// Вход: количество углерода кокса, пришедшего в печь (СПРИШ), кг/т.
    /// </summary>
    public double C { get; set; }

    /// <summary>
    /// Результат: переход углерода кокса в чугун (СЧ), кг/т чугуна.
    /// Расчёт — реализует пользователь.
    /// </summary>
    public double Cch => 10 * C;
}