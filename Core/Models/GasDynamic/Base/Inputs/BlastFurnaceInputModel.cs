using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models.GasDynamic.Base.Inputs;

public class BlastFurnaceInputModel : Entity
{
    [ForeignKey(nameof(Composition))] public int CompositionId { get; set; }

    [DataType(ReferenceDataType)] public CompositionParameters Composition { get; set; } = new();

    [ForeignKey(nameof(FuelAndBlast))] public int FuelAndBlastId { get; set; }

    [DataType(ReferenceDataType)] public FuelAndBlastParameters FuelAndBlast { get; set; } = new();

    [ForeignKey(nameof(Geometry))] public int GeometryId { get; set; }

    [DataType(ReferenceDataType)] public FurnaceGeometry Geometry { get; set; } = new();

    [ForeignKey(nameof(ThermalAndPressure))]
    public int ThermalAndPressureId { get; set; }

    [DataType(ReferenceDataType)] public ThermalAndPressureParameters ThermalAndPressure { get; set; } = new();

    [ForeignKey(nameof(Materials))] public int MaterialsId { get; set; }

    [DataType(ReferenceDataType)] public MaterialProperties Materials { get; set; } = new();

    [ForeignKey(nameof(Production))] public int ProductionId { get; set; }

    [DataType(ReferenceDataType)] public ProductionParameters Production { get; set; } = new();
}