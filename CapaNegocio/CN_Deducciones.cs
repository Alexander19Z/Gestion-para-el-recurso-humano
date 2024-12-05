using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Deducciones
    {
        private CD_Deducciones objCapaDato = new CD_Deducciones();

        public List<deducciones> listar()
        {
            return objCapaDato.listar();
        }


        public int registrar(deducciones obj, out string mensaje)
        {
            //Validaciones
            mensaje = string.Empty;

            //creacion de clave automatica
            return objCapaDato.registrar(obj, out mensaje);


        }

        public Boolean editar(deducciones obj, out string mensaje)
        {


            return objCapaDato.editar(obj, out mensaje);



        }

        public bool eliminar(int id, out string mensaje)
        {
            return objCapaDato.eliminar(id, out mensaje);
        }



    }
}
