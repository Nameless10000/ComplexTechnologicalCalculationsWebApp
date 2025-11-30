using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models.GasDynamic
{
    public class HydrodynamicsLower : EntityModel
    {
        public int BlastFurnanceOutputModelId { get; set; }
        
        [ForeignKey("BlastFurnanceOutputModelId")]
        public virtual BlastFurnanceOutputModel BlastFurnanceOutputModel { get; set; }
        
        [Display(Name = "Скорость газа в нижней части, м/с")]
        public double GasSpeedLower { get; set; }
        
        [Display(Name = "Давление в нижней части, кг/см²")]
        public double PressureLower { get; set; }
        
        [Display(Name = "Потери давления в нижней части, %")]
        public double PressureLossLower { get; set; }
        
        [Display(Name = "Коэффициент сопротивления нижней части")]
        public double ResistanceCoeffLower { get; set; }
        
        [Display(Name = "Объемный расход газа внизу, м3/мин")]
        public double VolumeFlowLower { get; set; }
        
        [Display(Name = "Температура газа внизу, °C")]
        public double GasTempLower { get; set; }
    }
}