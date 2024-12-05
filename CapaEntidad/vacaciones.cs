using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class vacaciones
    {
        public int id_Persona { get; set; }
        public decimal saldo_Vacaciones { get; set; }
        public string fecha_Modificacion { get; set; }
        public decimal dias_Ganado { get; set; }
        public decimal dias_Solicitado { get; set; }
        public decimal monto_Total { get; set; }
        public string nombre { get; set; }  
        public string apellido { get; set;}


    }
}
