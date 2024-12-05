using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Permiso
    {

        private CD_Permiso objCapaDato = new CD_Permiso();

        public List<permiso> listar()
        {
            return objCapaDato.listar();
        }


        public int registrar(permiso obj, out string mensaje)
        {
            //Validaciones
            mensaje = string.Empty;

            //creacion de clave automatica
            return objCapaDato.registrar(obj, out mensaje);


        }

        public Boolean editar(permiso obj, out string mensaje)
        {


            return objCapaDato.editar(obj, out mensaje);



        }

        public bool eliminar(int id, out string mensaje)
        {
            return objCapaDato.eliminar(id, out mensaje);
        }


    }
}
