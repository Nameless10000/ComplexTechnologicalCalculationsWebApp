using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models.GasDynamic
{
    public class AglomContent : KoksContent
    {
        [Display(Name = "Порозность, м3/м3")]
        public double Porosity { get; set; }
    }
}