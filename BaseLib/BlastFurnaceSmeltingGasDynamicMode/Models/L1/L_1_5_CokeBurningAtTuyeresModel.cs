namespace BaseLib.BlastFurnaceSmeltingGasDynamicMode.Models.L1;

/// <summary>
/// Л1.5 — Количество углерода кокса, сгорающего у фурм (СФ), кг/т чугуна
/// </summary>
public class L_1_5_CokeBurningAtTuyeresModel
{
    /// <summary>
    /// Вход: СПРИШ — количество углерода кокса пришедшего в печь, кг/т.
    /// </summary>
    public double Cprish { get; set; }

    /// <summary>
    /// Вход: расход на прямое восстановление CПР, кг/т.
    /// </summary>
    public double Cpr { get; set; }

    /// <summary>
    /// Вход: переход в чугун СЧ, кг/т.
    /// </summary>
    public double Cch { get; set; }

    /// <summary>
    /// Вход: углерод на образование CH4 (ССН4), кг/т.
    /// </summary>
    public double Cch4 { get; set; }

    /// <summary>
    /// Результат: количество углерода кокса, сгорающего у фурм (СФ), кг/т чугуна.
    /// Расчёт — реализует пользователь.
    /// </summary>
    public double Cf =>
        Cprish - (Cch + Cprish + Cpr);
}