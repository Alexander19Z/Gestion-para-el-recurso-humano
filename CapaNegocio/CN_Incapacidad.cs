using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Incapacidad
    {

        private CD_Incapacidad objCapaDato = new CD_Incapacidad();

        public List<incapacidad> listar()
        {
            return objCapaDato.listar();
        }


        public int registrar(incapacidad obj, out string mensaje)
        {
            //Validaciones
            mensaje = string.Empty;

            //creacion de clave automatica
            return objCapaDato.registrar(obj, out mensaje);


        }



        public bool aprobacion(int id_Persona)
        {
      

            //creacion de clave automatica
            return objCapaDato.aprobacion(id_Persona);


        }
        public Boolean editar(incapacidad obj, out string mensaje)
        {


            return objCapaDato.editar(obj, out mensaje);



        }

        public bool eliminar(int id, out string mensaje)
        {
            return objCapaDato.eliminar(id, out mensaje);
        }






    }
}
