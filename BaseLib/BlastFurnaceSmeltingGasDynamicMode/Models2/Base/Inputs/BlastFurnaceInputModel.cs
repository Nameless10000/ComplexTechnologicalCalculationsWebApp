namespace BaseLib.Models2.Base.Inputs;

public class BlastFurnaceInputModel
{
    public CompositionParameters Composition { get; set; } = new();
    public FuelAndBlastParameters FuelAndBlast { get; set; } = new();
    public FurnaceGeometry Geometry { get; set; } = new();
    public ThermalAndPressureParameters ThermalAndPressure { get; set; } = new();
    public MaterialProperties Materials { get; set; } = new();
    public ProductionParameters Production { get; set; } = new();
}