using BaseLib.Models2.Aglom.Outputs;
using BaseLib.Models2.Base.Outputs;

namespace BaseLib.Models2;

public class ResponseModelV2
{
    public AglomOutputModel AglomOutput { get; set; }

    public BlastFurnanceOutputModel BlastFurnance { get; set; }
}