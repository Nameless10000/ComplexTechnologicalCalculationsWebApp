namespace BaseLib.BlastFurnaceSmeltingGasDynamicMode.Models.L1;

/// <summary>
/// Л1.4 — Количество углерода кокса на образование CH4 (ССН4), кг/т чугуна
/// </summary>
public class L_1_4_CokeToCH4FormationModel
{
    /// <summary>
    /// Вход: СПРИШ — количество углерода кокса пришедшего в печь, кг/т.
    /// </summary>
    public double Cprish { get; set; }

    /// <summary>
    /// Результат: количество углерода на образование CH4, кг/т (ССН4).
    /// Расчёт — реализует пользователь.
    /// </summary>
    public double Cch4 => 0.008 * Cprish;
}