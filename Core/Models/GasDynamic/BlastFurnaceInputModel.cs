using System.ComponentModel.DataAnnotations.Schema;
using BaseLib.Models2;

namespace Core.Models.GasDynamic
{
    public class BlastFurnaceInputModel : EntityModel
    {
        public int RequestId { get; set; }

        [InverseProperty("BlastFurnaceInput")] public virtual RequestModelV2 Request { get; set; }

        public virtual CompositionParameters Composition { get; set; } = new();
        public virtual FuelAndBlastParameters FuelAndBlast { get; set; } = new();
        public virtual FurnaceGeometry Geometry { get; set; } = new();
        public virtual ThermalAndPressureParameters ThermalAndPressure { get; set; } = new();
        public virtual MaterialProperties Materials { get; set; } = new();
        public virtual ProductionParameters Production { get; set; } = new();
    }
}