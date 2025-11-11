using BaseLib.Models2.Aglom;
using BaseLib.Models2.Base.Inputs;

namespace BaseLib.Models2;

public class RequestModelV2
{
    public AglomInputModel AglomInput { get; set; }
    
    public BlastFurnaceInputModel BlastFurnaceInput { get; set; }
}