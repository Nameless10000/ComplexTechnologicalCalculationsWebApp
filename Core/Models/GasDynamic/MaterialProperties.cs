using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models.GasDynamic
{
    public class MaterialProperties : EntityModel
    {
        public int BlastFurnaceInputModelId { get; set; }
        
        [ForeignKey("BlastFurnaceInputModelId")]
        public virtual BlastFurnaceInputModel BlastFurnaceInputModel { get; set; }
        
        [Display(Name = "Порозность кускового слоя, м3/м3")]
        public double PorosityBulkLayer { get; set; }
        
        [Display(Name = "Порозность слоя кокса, м3/м3")]
        public double PorosityCokeLayer { get; set; }
        
        [Display(Name = "Насыпная плотность материала, кг/м3")]
        public double BulkDensity { get; set; }
        
        [Display(Name = "Удельная теплоемкость материала, кДж/(кг·К)")]
        public double SpecificHeatCapacity { get; set; }
        
        [Display(Name = "Коэффициент теплоотдачи, Вт/(м2·К)")]
        public double HeatTransferCoefficient { get; set; }
    }
}