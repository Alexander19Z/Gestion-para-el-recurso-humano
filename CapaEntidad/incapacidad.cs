using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class incapacidad
    {
        public int id_Incapacidad { get; set; }
        public string tipo_Incapacidad { get; set; }
        public string fecha_Inicio { get; set; }
        public string fecha_Solicitud { get; set; }
        public string fecha_Finalizacion { get; set; }
        public decimal monto_Apagar { get; set; }
        public string descripcion { get; set; }
        public int id_Persona { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public Boolean aprobado { get; set; }


    }
}
