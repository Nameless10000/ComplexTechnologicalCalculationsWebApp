using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models.GasDynamic
{
    public class HearthGas : EntityModel
    {
        public int BlastFurnanceOutputModelId { get; set; }
        
        [ForeignKey("BlastFurnanceOutputModelId")]
        public virtual BlastFurnanceOutputModel BlastFurnanceOutputModel { get; set; }
        
        [Display(Name = "Объем газа в горне, м3/мин")]
        public double VolumeGasHearth { get; set; }
        
        [Display(Name = "Температура газа в горне, °C")]
        public double TempGasHearth { get; set; }
        
        [Display(Name = "Состав CO в газе горна, %")]
        public double COContentHearth { get; set; }
        
        [Display(Name = "Состав CO2 в газе горна, %")]
        public double CO2ContentHearth { get; set; }
        
        [Display(Name = "Состав H2 в газе горна, %")]
        public double H2ContentHearth { get; set; }
        
        [Display(Name = "Давление газа в горне, кг/см²")]
        public double PressureGasHearth { get; set; }
        
        [Display(Name = "Скорость газа в горне, м/с")]
        public double SpeedGasHearth { get; set; }
    }
}