using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models.GasDynamic
{
    public class KoksContent : EntityModel
    {
        public int AglomInputModelId { get; set; }
        
        [ForeignKey("AglomInputModelId")]
        public virtual AglomInputModel AglomInputModel { get; set; }
        
        [Display(Name = "Минимальный размер фракции, мм")]
        public double MinFractionSize { get; set; }
        
        [Display(Name = "Содержание фракции, %")]
        public double FractionPercentage { get; set; }

        [Display(Name = "Доля фракции")]
        public double FractionPart => FractionPercentage / 100;
    }
}