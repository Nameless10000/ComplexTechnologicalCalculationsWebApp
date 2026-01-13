using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models.SlagMode;

public class Request : Entity
{
    [ForeignKey(nameof(CastIron))]
    public int CastIronID {get; set;}
    [ForeignKey(nameof(InputCoke))]
    public int CokeID {get; set;}
    [ForeignKey(nameof(Slag))]
    public int SlagID {get; set;}
    
    public CastIron CastIron {get; set;}
    public InputCoke InputCoke {get; set;}
    public Slag Slag {get; set;}
    public Response Response {get; set;}
    public List<ChargeComponent> Components {get; set;}
}