using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class planilla
    {
        public int id_Planilla { get; set;}
        public int id_Persona { get; set;}
        public decimal salario_Total { get; set;}
        public decimal deduccion_CCSS{ get; set;}
        public decimal deduccion_permiso { get; set; }
        public decimal deduccion_InpuestoRenta { get; set; }
        public decimal deduccion_Incapacidad { get; set; }
        public decimal deducciones_Varias { get; set; }
        public decimal salario_Pagar { get; set; }
        public string fecha_creacion { get; set; }
        public string fecha_Pago { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        

    }
}
