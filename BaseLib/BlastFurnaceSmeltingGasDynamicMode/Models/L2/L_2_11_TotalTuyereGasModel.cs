namespace BaseLib.BlastFurnaceSmeltingGasDynamicMode.Models.L2;

/// <summary>
/// Л2.11 — Суммарный выход фурменного (горнового) газа, м3/кг кокса
/// </summary>
public class L_2_11_TotalTuyereGasModel
{
    public double Vpg { get; set; }

    public double Cf { get; set; }

    public double Vg_ { get; set; }

    public double Vg__ { get; set; }

    public double Vgg => Vg_ + (Vpg / Cf) * Vg__;
}