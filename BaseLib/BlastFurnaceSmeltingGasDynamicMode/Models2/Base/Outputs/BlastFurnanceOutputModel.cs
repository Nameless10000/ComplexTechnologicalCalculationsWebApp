namespace BaseLib.Models2.Base.Outputs;

public class BlastFurnanceOutputModel
{
    public BlastParameters BlastParameters { get; set; }
    
    public CarbonBalance CarbonBalance { get; set; }
    
    public ChargeAndPacking ChargeAndPacking { get; set; }
    
    public FurnaceGeometry FurnaceGeometry { get; set; }
    
    public HearthGas HearthGas { get; set; }
    
    public HydrodynamicsLower HydrodynamicsLower { get; set; }
    
    public HydrodynamicsUpper HydrodynamicsUpper { get; set; }
    
    public IntermediateGas1000 IntermediateGas1000 { get; set; }
    
    public MaterialConsumption MaterialConsumption { get; set; }
    
    public ThermalParameters ThermalParameters { get; set; }
    
    public TopGas TopGas { get; set; }
}