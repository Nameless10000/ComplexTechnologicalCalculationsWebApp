using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models.GasDynamic
{
    public class CarbonBalance : EntityModel
    {
        public int BlastFurnanceOutputModelId { get; set; }
        
        [ForeignKey("BlastFurnanceOutputModelId")]
        public virtual BlastFurnanceOutputModel BlastFurnanceOutputModel { get; set; }
        
        [Display(Name = "Расход кокса на 1 т чугуна, кг")]
        public double Rashod_Koks { get; set; }
        
        [Display(Name = "Содержание углерода в коксе, %")]
        public double C_v_Kokse { get; set; }
        
        [Display(Name = "Расход углерода на восстановление, кг")]
        public double Rashod_C_Vosstanovlenie { get; set; }
        
        [Display(Name = "Расход углерода на карбюризацию, кг")]
        public double Rashod_C_Karbyurizatsiya { get; set; }
        
        [Display(Name = "Общий расход углерода, кг")]
        public double Rashod_C_Obshiy { get; set; }
        
        [Display(Name = "Коэффициент использования углерода")]
        public double Koeff_C_Ispolzovanie { get; set; }
    }
}