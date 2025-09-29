namespace BaseLib.BlastFurnaceSmeltingGasDynamicMode.Models.L2;

/// <summary>
/// Л2.9 — Выход фурменного газа в расчете на 1 кг кокса, м3/кг кокса
/// </summary>
public class L_2_9_TuyereGasPerKgCokeModel
{
    public double Vd_ { get; set; }

    public double O2 { get; set; }

    public double F { get; set; }

    public double Vg_ => 1.8667 + Vd_ * (1 - 0.01 * O2 + 0.00124 * F);
}