using System.ComponentModel.DataAnnotations;

namespace Core.Models.GasDynamic
{
    public class EntityModel
    {
        [Key]
        public int Id { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
    }
}