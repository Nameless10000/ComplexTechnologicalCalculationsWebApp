namespace BaseLib.BlastFurnaceSmeltingGasDynamicMode.Models.L2;

/// <summary>
/// Л2.10 — Выход фурменного газа при конверсии 1 м3 природного газа, м3/м3
/// </summary>
public class L_2_10_TuyereGasFromNaturalGasModel
{
    public double Vd__ { get; set; }

    public double O2 { get; set; }

    public double F { get; set; }

    public double Vg__ => 3 + Vd__ * (1 - 0.01 * O2 + 0.00124 * F);
}