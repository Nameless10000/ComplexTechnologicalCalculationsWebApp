using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console.DTO
{
    public class ComponentInfo
    {
        [DisplayName("Компонент шихты")]
        public string ComponentName { get; set; } // nickname 
        //даннные под табличку
        [DisplayName("Расход, кг/100")]
        public double ReportComponentOfShihta {get; set;}// отдельный компонент шихты
        [DisplayName("Fe")]
        public double ReportFe {get; set;}// железа в конкретном компоненте
        [DisplayName("S")]
        public double ReportS {get; set;}
        [DisplayName("P")]
        public double ReportP {get; set;}
        [DisplayName("FeO")]
        public double ReportFeO {get; set;}
        [DisplayName("CaO")]
        public double ReportCaO {get; set; }
        [DisplayName("SiO2")]
        public double ReportSiO2 { get; set; }
        [DisplayName("Al2O3")]
        public double ReportAl2O3 {get; set;}
        [DisplayName("MgO")]
        public double ReportMgO {get; set;}
        [DisplayName("MnO")]
        public double ReportMnO {get; set; }
        [DisplayName("TiO2")]
        public double ReportTiO2 { get; set; }
        [DisplayName("Zn")]
        public double ReportZn { get; set; }
        [DisplayName("ПМПП")]
        public double ReportPMPP { get; set; }

        //доп даннные под конечную табличку
        [DisplayName("Fe2O3 в хим. составе")]
        public double ReportFe2O3 { get; set; } // Оксид железа III в хим составе 
        [DisplayName("Сумма оксидов")]
        public double ReportOxideSum { get; set; } // Сумма оксидов в хим состве
        [DisplayName("Основность CaO/SiO2")]
        public double ReportCaO_SiO2 { get; set; } // Итоговая основность агломерата
    }
}
