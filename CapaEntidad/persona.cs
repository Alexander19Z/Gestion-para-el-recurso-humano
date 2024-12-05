using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    /*
     
        id_Persona int not null primary key identity(1,1),
        nombre varchar(100) not null, 
        apellido varchar(100) not null,
        cedula varchar(100) not null,
        sexo varchar(50),
        usuario varchar(50) not null,
        correo_Personal varchar(100)  not null,
        contrasena varchar(500) not null,
        correo_Empresarial varchar(100)not null,
        activo bit default(1)not null,
        fecha_ingreso date not null,
        fecha_salida date,
        tipo_Usuario varchar(100) not null
     
     */




    public class persona
    {
        public int id_Persona { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set;}
        public string apellidoS { get; set; }
        public string cedula { get; set; }
        public string sexo { get; set; }
        public string usuario { get; set; }
      
        public string contrasena { get; set; }
        public string correo_Empresarial { get; set; }
        public bool activo { get; set; }
        public DateTime fecha_ingreso { get; set; }
        public DateTime fecha_salida { get; set; }
        public string tipo_Usuario { get; set; }
        public int id_Departamento { get; set; } 
        public int id_Puesto { get; set; }
        public  int cantidad_Hijos { get; set; }
        public string estado_Civil { get; set; }
        public  string telefono { get; set; }
        public string  direccion { get; set; }
        public string departamentoDescripcion { get; set; }
        public string puestoDescripcion { get;set;}
        public string canton { get; set; }
        public string distrito { get; set; }
        public string provincia { get; set; }








    }
}
