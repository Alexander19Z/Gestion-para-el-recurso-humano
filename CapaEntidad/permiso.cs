using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class permiso
    {
        public int id_Permiso { get; set; }
        public string fecha_Solicitud { get; set; }
        public string hora_Inicio { get; set; }
        public string hora_Finalizacion { get; set; }
        public string concepto { get; set; }
        public string descripcion { get; set; }
        public int id_Persona { get; set; }
        public Boolean aprobado { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }

    }
}
