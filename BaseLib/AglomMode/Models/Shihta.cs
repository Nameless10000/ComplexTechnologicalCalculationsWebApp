using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Console
{
    public class Shihta
    {
        public double TotalPartOfWet { get; set; }
        public double TotalPartOfPMPP { get; set; }
        public double TotalPercentOfPMPP { get; set; }

        #region Total Chemistry
        public double TotalFeMass { get; set; }
        public double TotalFeOMass { get; set; }
        public double TotalCaOMass { get; set; }
        public double TotalSiO2Mass { get; set; }
        public double TotalMgOMass { get; set; }
        public double TotalAl2O3Mass { get; set; }
        public double TotalTiO2Mass { get; set; }
        public double TotalSMass { get; set; }
        public double TotalPMass { get; set; }
        public double TotalCrMass { get; set; }
        public double TotalZnMass { get; set; }
        public double TotalMnOMass { get; set; }

        #endregion

    }                 
}
