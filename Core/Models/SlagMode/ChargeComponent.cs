using System.Text.Json.Serialization;

namespace Core.Models.SlagMode;

public class ChargeComponent : Entity
{
    public string Sourcename { get; set; }

    public double Consumption { get; set; }

    public double Fe { get; set; }

    public double SiO2 { get; set; }

    public double Al2O3 { get; set; }

    public double CaO { get; set; }

    public double MgO { get; set; }

    public double S { get; set; }

    public double MnO { get; set; }

    public double TiO2 { get; set; }
    [JsonIgnore]
    public List<Request> Requests {get; set;}
}