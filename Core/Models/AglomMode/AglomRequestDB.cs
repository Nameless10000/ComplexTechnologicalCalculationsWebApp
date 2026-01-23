using Console;
using Core.Models.SlagMode;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.AglomMode
{
    public class AglomRequestDB : Entity
    {
        [ForeignKey(nameof(ZolaOfCocksick))]
        public int ZolaOfCocksickID { get; set; }
        [ForeignKey(nameof(Cocksick))]
        public int CocksickID { get; set; }
        [ForeignKey(nameof(FluxAdditions))]
        public int FluxAdditionsID { get; set; }
        [ForeignKey(nameof(StartEnter))]
        public int StartEnterID { get; set; }
        [ForeignKey(nameof(AglomResponse))]
        public int AglomResponseID {  get; set; }


        public ZolaOfCocksickDB ZolaOfCocksick { get; set; }
        public CocksickDB Cocksick { get; set; }
        public FluxAdditionsDB FluxAdditions { get; set; }
        public StartEnterDB StartEnter { get; set; }

        public List<ShihtaComponentDB> ShihtaComponents { get; set; }
        public AglomResponseDB AglomResponse { get; set; }
    }
}
