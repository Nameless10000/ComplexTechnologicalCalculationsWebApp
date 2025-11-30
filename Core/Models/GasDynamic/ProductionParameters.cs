using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models.GasDynamic
{
    public class ProductionParameters : EntityModel
    {
        public int BlastFurnaceInputModelId { get; set; }
        
        [ForeignKey("BlastFurnaceInputModelId")]
        public virtual BlastFurnaceInputModel BlastFurnaceInputModel { get; set; }
        
        [Display(Name = "Производительность, т/сутки")]
        public double Productivity { get; set; }
        
        [Display(Name = "Количество дней работы")]
        public int DaysOfWork { get; set; }
        
        [Display(Name = "Количество остановок")]
        public int CountStops { get; set; }
        
        [Display(Name = "Продолжительность остановки, ч")]
        public double DurationStop { get; set; }
    }
}