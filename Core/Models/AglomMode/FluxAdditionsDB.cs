using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.AglomMode
{
    public class FluxAdditionsDB : Entity
    {
        #region Izvestnyak
        public double IzvestnyakCaO { get; set; }
        public double IzvestnyakSiO2 { get; set; }
        public double IzvestnyakAl2O3 { get; set; }
        public double IzvestnyakMgO { get; set; }
        public double IzvestnyakPMPP { get; set; }

        #endregion

        #region Dolomyte
        public double DolomyteCaO { get; set; }
        public double DolomyteSiO2 { get; set; }
        public double DolomyteAl2O3 { get; set; }
        public double DolomyteMgO { get; set; }
        public double DolomytePMPP { get; set; }

        #endregion
    }
}
