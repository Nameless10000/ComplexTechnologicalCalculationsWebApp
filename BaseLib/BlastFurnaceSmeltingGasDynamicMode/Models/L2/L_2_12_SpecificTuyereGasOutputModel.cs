namespace BaseLib.BlastFurnaceSmeltingGasDynamicMode.Models.L2;

/// <summary>
/// Л2.12 — Удельный выход фурменного (горнового) газа, м3/т чугуна
/// </summary>
public class L_2_12_SpecificTuyereGasOutputModel
{
    public double Vgg { get; set; }

    public double Cf { get; set; }

    public double Qgg => Cf * Vgg;
}