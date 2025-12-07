namespace Core.Models.GasDynamic.Aglom.Inputs;

public class AglomInputModel : Entity
{
    public List<KoksContent> KoksContents { get; set; }

    public List<AglomContent> AglomContents { get; set; }

    public List<OkatContent> OkatContents { get; set; }
}