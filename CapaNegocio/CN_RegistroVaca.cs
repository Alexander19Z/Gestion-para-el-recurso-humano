using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_RegistroVaca
    {

            private CD_RegistroVaca objCapaDato = new CD_RegistroVaca ();
        private CD_ReporteRegustroVaca objCapaDatoR = new CD_ReporteRegustroVaca();

        public List<registro_Vaca> listar()
            {
                return objCapaDato.listar();
            }


        public List<registro_Vaca> ReporteListar(int id_Persona) {
        
        return objCapaDatoR.listar(id_Persona);
        }


            public int registrar(registro_Vaca obj, out string mensaje)
            {
                //Validaciones
                mensaje = string.Empty;

                //creacion de clave automatica
                return objCapaDato.registrar(obj, out mensaje);


            }

            public Boolean editar(registro_Vaca obj, out string mensaje)
            {


                return objCapaDato.editar(obj, out mensaje);



            }

            public bool eliminar(int id, out string mensaje)
            {
                return objCapaDato.eliminar(id, out mensaje);
            }


        }


    }

