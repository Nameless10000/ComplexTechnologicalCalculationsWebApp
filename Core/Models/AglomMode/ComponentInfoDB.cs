using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.AglomMode
{
    public class ComponentInfoDB : Entity
    {
        public string ComponentName { get; set; } // nickname 
                                                  //даннные под табличку
        public double ReportComponentOfShihta { get; set; }// отдельный компонент шихты
        public double ReportFe { get; set; }// железа в конкретном компоненте
        public double ReportS { get; set; }
        public double ReportP { get; set; }
        public double ReportFeO { get; set; }
        public double ReportCaO { get; set; }
        public double ReportSiO2 { get; set; }
        public double ReportAl2O3 { get; set; }
        public double ReportMgO { get; set; }
        public double ReportMnO { get; set; }
        public double ReportTiO2 { get; set; }
        public double ReportZn { get; set; }
        public double ReportPMPP { get; set; }

        //доп даннные под конечную табличку
        public double ReportFe2O3 { get; set; } // Оксид железа III в хим составе 
        public double ReportOxideSum { get; set; } // Сумма оксидов в хим состве
        public double ReportCaO_SiO2 { get; set; } // Итоговая основность агломерата

        public AglomResponseDB AglomResponse { get; set; }
        [ForeignKey(nameof(AglomResponse))]
        public int AglomResponseID { get; set; }
    }
}
