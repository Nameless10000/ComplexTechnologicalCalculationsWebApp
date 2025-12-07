using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Models.GasDynamic.Aglom.Outputs;
using Core.Models.GasDynamic.Base.Outputs;

namespace Core.Models.GasDynamic;

public class ResponseModelV2 : Entity
{
    [ForeignKey(nameof(AglomOutput))] public int AglomOutputId { get; set; }

    [DataType(ReferenceDataType)] public AglomOutputModel AglomOutput { get; set; }

    [ForeignKey(nameof(BlastFurnanceOutput))]
    public int BlastFurnanceOutputId { get; set; }

    [DataType(ReferenceDataType)] public BlastFurnanceOutputModel BlastFurnanceOutput { get; set; }

    [ForeignKey(nameof(RequestModel))] public int RequestModelId { get; set; }

    [DataType(ReferenceDataType)] public RequestModelV2 RequestModel { get; set; }
}