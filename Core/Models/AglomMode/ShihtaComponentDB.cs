using Console;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.AglomMode
{
    public class ShihtaComponentDB : Entity
    {
        #region Osnova
        public string Name { get; set; }
        public double Weight { get; set; }
        public double Wet { get; set; }
        public double PMPP { get; set; }

        #endregion

        #region Chemical Components
        public double Fe { get; set; }
        public double FeO { get; set; }
        public double CaO { get; set; }
        public double SiO2 { get; set; }
        public double MgO { get; set; }
        public double Al2O3 { get; set; }
        public double TiO2 { get; set; }
        public double S { get; set; }
        public double P { get; set; }
        public double Cr { get; set; }
        public double Zn { get; set; }
        public double MnO { get; set; }

        #endregion
        [ForeignKey(nameof(AglomRequest))]
        public int AglomRequestID { get; set; }
        public AglomRequestDB AglomRequest { get; set; }
    }
}
