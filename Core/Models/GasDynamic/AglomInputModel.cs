using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models.GasDynamic
{
    public class AglomInputModel : EntityModel
    {
        public int RequestId { get; set; }
        
        [InverseProperty("AglomInput")]
        public virtual RequestModelV2 Request { get; set; }
        
        public virtual ICollection<KoksContent> KoksContents { get; set; } = new List<KoksContent>();
        
        public virtual ICollection<AglomContent> AglomContents { get; set; } = new List<AglomContent>();
        
        public virtual ICollection<OkatContent> OkatContents { get; set; } = new List<OkatContent>();
    }
}