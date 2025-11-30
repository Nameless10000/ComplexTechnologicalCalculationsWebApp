using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models.GasDynamic
{
    public class FuelAndBlastParameters : EntityModel
    {
        public int BlastFurnaceInputModelId { get; set; }
        
        [ForeignKey("BlastFurnaceInputModelId")]
        public virtual BlastFurnaceInputModel BlastFurnaceInputModel { get; set; }
        
        [Display(Name = "Удельный расход кокса, кг/т чугуна")]
        public double Udeln_koks { get; set; }

        [Display(Name = "Содержание нелетучего углерода в коксе, %")]
        public double C_neletuch { get; set; }

        [Display(Name = "Степень прямого восстановления (rd)")]
        public double Stepen_pryamogo_vost { get; set; }

        [Display(Name = "Содержание кислорода в дутье, %")]
        public double Kislorod_dut { get; set; }

        [Display(Name = "Влажность дутья, г/м³")]
        public double Vlazhn_dut { get; set; }

        [Display(Name = "Удельный расход природного газа, м³/т чугуна")]
        public double Udeln_prir_gaz { get; set; }

        [Display(Name = "Содержание C в природном газе (CH₄), м³/м³")]
        public double C_prir_gaz { get; set; }

        [Display(Name = "Содержание H₂ в природном газе (2CH₄), м³/м³")]
        public double H2_prir_gaz { get; set; }

        [Display(Name = "Степень использования водорода (nH₂)")]
        public double Stepen_vodorod { get; set; }

        [Display(Name = "Степень использования CO (nCO)")]
        public double Stepen_CO { get; set; }

        [Display(Name = "Минутный расход дутья, м³/мин")]
        public double Rashod_dut { get; set; }

        [Display(Name = "Потери давления горячего дутья, %")]
        public double Poteri_dut { get; set; }
    }
}