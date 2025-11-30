using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models.GasDynamic
{
    public class BlastFurnanceOutputModel : EntityModel
    {
        public int ResponseId { get; set; }
        
        [InverseProperty("BlastFurnanceOutputModel")]
        public virtual ResponseModelV2 Response { get; set; }
        
        public virtual BlastParameters BlastParameters { get; set; }
        
        public virtual CarbonBalance CarbonBalance { get; set; }
        
        public virtual ChargeAndPacking ChargeAndPacking { get; set; }
        
        public virtual FurnaceGeometryOutput FurnaceGeometry { get; set; }
        
        public virtual HearthGas HearthGas { get; set; }
        
        public virtual HydrodynamicsLower HydrodynamicsLower { get; set; }
        
        public virtual HydrodynamicsUpper HydrodynamicsUpper { get; set; }
        
        public virtual IntermediateGas1000 IntermediateGas1000 { get; set; }
        
        public virtual MaterialConsumption MaterialConsumption { get; set; }
        
        public virtual ThermalParameters ThermalParameters { get; set; }
        
        public virtual TopGas TopGas { get; set; }
    }
}