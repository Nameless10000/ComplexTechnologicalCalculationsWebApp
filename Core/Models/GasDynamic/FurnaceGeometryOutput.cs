using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models.GasDynamic
{
    public class FurnaceGeometryOutput : EntityModel
    {
        public int BlastFurnanceOutputModelId { get; set; }
        
        [ForeignKey("BlastFurnanceOutputModelId")]
        public virtual BlastFurnanceOutputModel BlastFurnanceOutputModel { get; set; }
        
        [Display(Name = "Фактическая высота шахты, м")]
        public double ActualHeightShaft { get; set; }
        
        [Display(Name = "Фактическая высота распара, м")]
        public double ActualHeightRaspar { get; set; }
        
        [Display(Name = "Фактическая высота горна, м")]
        public double ActualHeightGorn { get; set; }
        
        [Display(Name = "Фактический диаметр шахты, м")]
        public double ActualDiamShaft { get; set; }
        
        [Display(Name = "Фактический диаметр распара, м")]
        public double ActualDiamRaspar { get; set; }
        
        [Display(Name = "Фактический диаметр горна, м")]
        public double ActualDiamGorn { get; set; }
        
        [Display(Name = "Изменение геометрии, %")]
        public double GeometryChangePercent { get; set; }
    }
}