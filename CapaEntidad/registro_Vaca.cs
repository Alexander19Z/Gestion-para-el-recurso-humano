using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class registro_Vaca
    {
        public int id_Registro { get; set; }
        public char tipo_Registro { get; set; }
        public int id_Persona { get; set; }
        public string fecha_Registro { get; set; }
        public string tipo_Movimiento { get; set; }
        public string periodo { get; set; }

        public string nombre { get; set; }
        public string apellido { get; set; }
    }
}

