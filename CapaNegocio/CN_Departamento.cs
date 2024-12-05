using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Departamento
    {
        private CD_Departamento objCapaDato = new CD_Departamento();

        public List<departamento> listar()
        {
            return objCapaDato.listar();
        }


        public int registrar(departamento obj, out string mensaje)
        {
            string res = validaciones(obj);
            if (string.IsNullOrEmpty(res))
            {
                return objCapaDato.registrar(obj, out mensaje);
            }
            else
            {
                mensaje = res;
                return 0;

            }

        }

        public Boolean editar(departamento obj, out string mensaje)
        {


            string res = validaciones(obj);
            if (string.IsNullOrEmpty(res))
            {
                return objCapaDato.editar(obj, out mensaje);
            }
            else
            {
                mensaje = res;
                return false;

            }



        }

        public bool eliminar(int id, out string mensaje)
        {
            return objCapaDato.eliminar(id, out mensaje);
        }


        public string validaciones(departamento obj)
        {
            string mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.descripcion) || string.IsNullOrWhiteSpace(obj.descripcion))
            {
                mensaje = "El campo descripción no puede estar en blanco.";
            }

            





            return mensaje;
        }



    }
}
