using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models.GasDynamic.Base.Outputs;

public class BlastFurnanceOutputModel : Entity
{
    [ForeignKey(nameof(BlastParameters))] public int BlastParametersId { get; set; }

    [DataType(ReferenceDataType)] public BlastParameters BlastParameters { get; set; }

    [ForeignKey(nameof(CarbonBalance))] public int CarbonBalanceId { get; set; }

    [DataType(ReferenceDataType)] public CarbonBalance CarbonBalance { get; set; }

    [ForeignKey(nameof(ChargeAndPacking))] public int ChargeAndPackingId { get; set; }

    [DataType(ReferenceDataType)] public ChargeAndPacking ChargeAndPacking { get; set; }

    [ForeignKey(nameof(FurnaceGeometry))] public int FurnaceGeometryId { get; set; }

    [DataType(ReferenceDataType)] public FurnaceGeometry FurnaceGeometry { get; set; }

    [ForeignKey(nameof(HearthGas))] public int HearthGasId { get; set; }

    [DataType(ReferenceDataType)] public HearthGas HearthGas { get; set; }

    [ForeignKey(nameof(HydrodynamicsLower))]
    public int HydrodynamicsLowerId { get; set; }

    [DataType(ReferenceDataType)] public HydrodynamicsLower HydrodynamicsLower { get; set; }

    [ForeignKey(nameof(HydrodynamicsUpper))]
    public int HydrodynamicsUpperId { get; set; }

    [DataType(ReferenceDataType)] public HydrodynamicsUpper HydrodynamicsUpper { get; set; }

    [ForeignKey(nameof(IntermediateGas1000))]
    public int IntermediateGas1000Id { get; set; }

    [DataType(ReferenceDataType)] public IntermediateGas1000 IntermediateGas1000 { get; set; }

    [ForeignKey(nameof(MaterialConsumption))]
    public int MaterialConsumptionId { get; set; }

    [DataType(ReferenceDataType)] public MaterialConsumption MaterialConsumption { get; set; }

    [ForeignKey(nameof(ThermalParameters))]
    public int ThermalParametersId { get; set; }

    [DataType(ReferenceDataType)] public ThermalParameters ThermalParameters { get; set; }

    [ForeignKey(nameof(TopGas))] public int TopGasId { get; set; }

    [DataType(ReferenceDataType)] public TopGas TopGas { get; set; }
}