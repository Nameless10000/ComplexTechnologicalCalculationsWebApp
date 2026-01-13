using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class Entity
    {
        public const string ReferenceDataType = "Reference";

        [Key] public int Id { get; set; }

        public DateTime CreationDateTime { get; set; }
        public int CreatorID { get; set; }
        public DateTime? LastEditedDateTime { get; set; }
        public int? LastEditorID { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public int? DeletedByID { get; set; }
    }
}