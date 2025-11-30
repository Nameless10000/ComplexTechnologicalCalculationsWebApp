using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models.GasDynamic
{
    public class ChargeAndPacking : EntityModel
    {
        public int BlastFurnanceOutputModelId { get; set; }
        
        [ForeignKey("BlastFurnanceOutputModelId")]
        public virtual BlastFurnanceOutputModel BlastFurnanceOutputModel { get; set; }
        
        [Display(Name = "Высота шахты, м")]
        public double HeightShaft { get; set; }
        
        [Display(Name = "Высота распара, м")]
        public double HeightRaspar { get; set; }
        
        [Display(Name = "Высота горна, м")]
        public double HeightGorn { get; set; }
        
        [Display(Name = "Порозность шахты, м3/м3")]
        public double PorosityShaft { get; set; }
        
        [Display(Name = "Порозность распара, м3/м3")]
        public double PorosityRaspar { get; set; }
        
        [Display(Name = "Порозность горна, м3/м3")]
        public double PorosityGorn { get; set; }
        
        [Display(Name = "Плотность набивки, т/м3")]
        public double DensityPacking { get; set; }
        
        [Display(Name = "Коэффициент заполнения")]
        public double KoeffZapolneniya { get; set; }
    }
}