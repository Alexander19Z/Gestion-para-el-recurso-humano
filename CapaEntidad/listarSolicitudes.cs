using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class listarSolicitudes
    {
        public int id_Noti { get; set; }
        public string tipo_Notificacion { get; set; }
        public bool estado { get; set; }
        public int id_Persona { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string fechasSolicitadas { get; set; }
        public decimal dias_Solicitado { get; set; }
        public decimal horas_solicitadas { get; set; }
        public string descripcion { get; set; }
        public Boolean aprobacionRRHH { get; set;}
    }
}
