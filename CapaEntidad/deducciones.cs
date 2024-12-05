using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class deducciones
    {
        public  int id_Deduccion { get; set; }
        public string tipo_Deduccion { get; set;}
        public decimal cantidad_Cuotas { get; set; }
        public  string fecha_Inicio { get; set; }
        public string fecha_Finalizacion { get; set; }
        public decimal monto_Cuota { get; set; }
        public Boolean activo { get; set; }
        public int id_Persona { get; set; }
        public  string  nombre { get; set; }
        public string apellido { get; set; }
    }
}
