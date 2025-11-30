using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models.GasDynamic
{
    public class BlastParameters : EntityModel
    {
        public int BlastFurnanceOutputModelId { get; set; }
        
        [ForeignKey("BlastFurnanceOutputModelId")]
        public virtual BlastFurnanceOutputModel BlastFurnanceOutputModel { get; set; }
        
        [Display(Name = "Расход сухого дутья на 1 кг С кокса")]
        public double Rashod_Dut_Koks { get; set; }

        [Display(Name = "Расход сухого дутья для конверсии 1 м3 прир. газа.")]
        public double Rashod_Dut_Prir_Gaz { get; set; }

        [Display(Name = "Суммарный расход сухого дутья")]
        public double Rashod_Dut_Sum { get; set; }

        [Display(Name = "Расчетный удельный расход дутья")]
        public double Rashod_Dut_Udeln { get; set; }

        [Display(Name = "Расчетное значение минутного расхода дутья")]
        public double Rashod_Dut_Minut { get; set; }

        [Display(Name = "Скорость истечения дутья из фурмы")]
        public double Speed_Dut_Furm { get; set; }

        [Display(Name = "Кинематическая вязкость дутья")]
        public double Vyazkost_Dut { get; set; }

        [Display(Name = "Значение критерия Рейнольдса")]
        public double Reinolds { get; set; }

        [Display(Name = "Критический расход дутья")]
        public double Rashod_Dut_Krit { get; set; }
    }
}