using Console;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLib.AglomMode.Models
{
    public class AglomRequestData
    {
        public int UserId { get; set; } = 1; // todo проверить
        public ZolaOfCocksick ZolaOfCocksick { get; set; }
        public Cocksick Cocksick { get; set; }
        public FluxAdditions FluxAdditions { get; set; }
        public List<ShihtaComponent> ShihtaComponents { get; set; }
        public StartEnter StartEnter { get; set; }
    }
}

