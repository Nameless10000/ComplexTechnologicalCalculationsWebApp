using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models.GasDynamic.Aglom.Inputs;

public class OkatContent : Entity
{
    [DisplayName("Порозность, м3/м3")] public double Porosity { get; set; }

    [DisplayName("Минимальный размер фракции, мм")]
    public double MinFractionSize { get; set; }

    [DisplayName("Содержание фракции, %")] public double FractionPercentage { get; set; }

    [DisplayName("Доля фракции")] public double FractionPart => FractionPercentage / 100;

    [ForeignKey(nameof(AglomInputModel))] public int AglomInputModelId { get; set; }

    [DataType(ReferenceDataType)] public AglomInputModel AglomInputModel { get; set; }
}