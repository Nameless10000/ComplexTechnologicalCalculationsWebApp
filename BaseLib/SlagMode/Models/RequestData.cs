using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLib.SlagMode.Models
{
    public class RequestData
    {
        public UserAuthData User { get; set; }

        public InputCokeForCalcs Coke { get; set; }

        public InputCastIronForCalc Iron { get; set; }

        public InputSlagForCalc Slag { get; set; }

        public List<InputChargeComponentsForCalc> Components { get; set; }
    }
}
