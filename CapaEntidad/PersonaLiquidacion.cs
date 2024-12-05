using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class PersonaLiquidacion
    {

        public int id_Persona { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string cedula { get; set; }
        public  string fecha_ingreso { get; set; }
        public decimal salario_Bruto { get; set; }
        public decimal saldo_Vacaciones { get; set; }
    }
}
