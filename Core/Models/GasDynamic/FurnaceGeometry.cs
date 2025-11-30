using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models.GasDynamic
{
    public class FurnaceGeometry : EntityModel
    {
        public int BlastFurnaceInputModelId { get; set; }
        
        [ForeignKey("BlastFurnaceInputModelId")]
        public virtual BlastFurnaceInputModel BlastFurnaceInputModel { get; set; }
        
        [Display(Name = "Высота колошника, м")]
        public double HeightKoloshnik { get; set; }
        
        [Display(Name = "Высота шахты, м")]
        public double HeightShaft { get; set; }
        
        [Display(Name = "Высота распара, м")]
        public double HeightRaspar { get; set; }
        
        [Display(Name = "Высота горна, м")]
        public double HeightGorn { get; set; }
        
        [Display(Name = "Высота поддойника, м")]
        public double HeightPoddoy { get; set; }
        
        [Display(Name = "Диаметр колошника, м")]
        public double DiamKoloshnik { get; set; }
        
        [Display(Name = "Диаметр шахты, м")]
        public double DiamShaft { get; set; }
        
        [Display(Name = "Диаметр распара, м")]
        public double DiamRaspar { get; set; }
        
        [Display(Name = "Диаметр горна, м")]
        public double DiamGorn { get; set; }
        
        [Display(Name = "Количество фурм")]
        public int CountFurms { get; set; }
        
        [Display(Name = "Количество леток")]
        public int CountLadles { get; set; }
    }
}