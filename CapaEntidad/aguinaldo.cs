using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class aguinaldo
    {
        public int id_Aguinaldo { get; set; }
        public decimal monto_Total { get; set; }
        public DateTime fecha_Pago { get; set; }
        public int id_Persona { get; set; }

        public string nombre { get; set; }
        public string apellido { get; set; }
    }
}
