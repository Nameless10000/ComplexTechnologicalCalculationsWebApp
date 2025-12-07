using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Models.GasDynamic.Aglom.Inputs;
using Core.Models.GasDynamic.Base.Inputs;

namespace Core.Models.GasDynamic;

public class RequestModelV2 : Entity
{
    [DataType(ReferenceDataType)] public AglomInputModel AglomInput { get; set; }

    [ForeignKey(nameof(AglomInput))] public int AglomInputId { get; set; }

    [DataType(ReferenceDataType)] public BlastFurnaceInputModel BlastFurnaceInput { get; set; }

    [ForeignKey(nameof(BlastFurnaceInput))]
    public int BlastFurnaceInputId { get; set; }

    public bool IsPreset { get; set; } = false;
}