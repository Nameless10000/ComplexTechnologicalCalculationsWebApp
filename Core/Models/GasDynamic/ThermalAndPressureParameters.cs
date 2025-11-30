using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models.GasDynamic
{
    public class ThermalAndPressureParameters : EntityModel
    {
        public int BlastFurnaceInputModelId { get; set; }
        
        [ForeignKey("BlastFurnaceInputModelId")]
        public virtual BlastFurnaceInputModel BlastFurnaceInputModel { get; set; }
        
        [Display(Name = "Температура дутья, °C")]
        public double TempDut { get; set; }
        
        [Display(Name = "Температура чугуна, °C")]
        public double TempChugun { get; set; }
        
        [Display(Name = "Температура шлака, °C")]
        public double TempShlak { get; set; }
        
        [Display(Name = "Давление на колошнике, кг/см²")]
        public double PressureKoloshnik { get; set; }
        
        [Display(Name = "Давление в ковшах, кг/см²")]
        public double PressureLadles { get; set; }
    }
}