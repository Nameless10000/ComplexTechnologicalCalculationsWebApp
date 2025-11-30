using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models.GasDynamic
{
    public class CompositionParameters : EntityModel
    {
        public int BlastFurnaceInputModelId { get; set; }
        
        [ForeignKey("BlastFurnaceInputModelId")]
        public virtual BlastFurnaceInputModel BlastFurnaceInputModel { get; set; }
        
        [Display(Name = "Содержание железа [Fe], %")]
        public double Fe_chugun { get; set; }

        [Display(Name = "Содержание марганца [Mn], %")]
        public double Mn_chugun { get; set; }

        [Display(Name = "Содержание фосфора [P], %")]
        public double P_chugun { get; set; }

        [Display(Name = "Содержание кремния [Si], %")]
        public double Si_chugun { get; set; }

        [Display(Name = "Содержание серы (S), %")]
        public double S_shlak { get; set; }

        [Display(Name = "Содержание углерода [C], %")]
        public double C_chugun { get; set; }
    }
}