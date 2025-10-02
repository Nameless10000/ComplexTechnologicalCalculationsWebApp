using Core.Models.Auth;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Entity
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreationDateTime { get; set; }
        public int CreatorID { get; set; }
        public DateTime? LastEditTime { get; set; }
        public int? LastEditorID { get; set; }
    }
}
