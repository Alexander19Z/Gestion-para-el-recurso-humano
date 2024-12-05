using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class reporteLiquidacion
    {
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string cedula { get; set; }
        public int id_Liquidacion { get; set; }
        public string descripcion { get; set; }
        public decimal monto_pagar { get; set; }
        public string tipo_Liquidacion { get; set; }
        public int id_Persona { get; set; }
        public decimal vacacionesNoDistrutadas { get; set; }
        public decimal cesantia { get; set; }
        public decimal aguinaldo { get; set; }
        public decimal preaviso { get; set; }

    }
}
