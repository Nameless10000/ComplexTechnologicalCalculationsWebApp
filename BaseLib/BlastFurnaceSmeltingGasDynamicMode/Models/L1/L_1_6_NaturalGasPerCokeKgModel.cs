namespace BaseLib.BlastFurnaceSmeltingGasDynamicMode.Models.L1;

/// <summary>
/// Л1.6 — Расход природного газа в расчете на 1 кг углерода кокса, сгорающего у фурм (Sigma), м3/кг кокса
/// </summary>
public class L_1_6_NaturalGasPerCokeKgModel
{
    /// <summary>
    /// Вход: объём природного газа, используемый на 1 т чугуна (VПГ), м3/т — или иной базовый объём (уточнить единицы).
    /// </summary>
    public double Vpg { get; set; }

    /// <summary>
    /// Вход: СФ — количество углерода, сгорающего у фурм, кг/т.
    /// </summary>
    public double Cf { get; set; }

    /// <summary>
    /// Результат: расход природного газа в расчёте на 1 кг сгорающего у фурм кокса (Sigma), м3/кг.
    /// Формула: Sigma = VПГ / СФ — реализация пользователем.
    /// </summary>
    public double Sigma => Vpg / Cf;
}