using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Vacaciones
    {


        private CD_Vacaciones objCapaDato = new CD_Vacaciones();
        private CD_Notificacion objCapaDatoNoti = new CD_Notificacion();

        public List<vacaciones> listar()
        {
            return objCapaDato.listar();
        }

        public List<vacaciones> saldos(int id)
        {
            return objCapaDato.saldos(id);
        }


        public int registrar(vacaciones obj, out string mensaje)
        {
            //Validaciones
            mensaje = string.Empty;

            //creacion de clave automatica
            return objCapaDato.registrar(obj, out mensaje);


        }

        public Boolean editar(vacaciones obj, out string mensaje)
        {


            return objCapaDato.editar(obj, out mensaje);



        }

        public bool eliminar(int id, out string mensaje)
        {
            return objCapaDato.eliminar(id, out mensaje);
        }

        public bool MontoEnDineroVacaciones(int persona, decimal salarioDiario) {
            return objCapaDato.MontoEnDineroVacaciones(persona, salarioDiario);
        }

        public int actualizarVacaciones(string nombre, decimal diasSolicitados)
        {

            return objCapaDato.actualizarVacaciones(nombre, diasSolicitados);
        }



        public Boolean VerificarNotificacion(int idPersona, string TipoNoti)
        {

            List<notificaciones> listaNoti = (List<notificaciones>)objCapaDatoNoti.listar();
            var NotificacionActiva = listaNoti.FirstOrDefault(b => b.id_Persona == idPersona && b.tipo_Notificacion == TipoNoti && b.estado == false);
            if (NotificacionActiva != null)
            {
                return true;
            }
            
            return false;
        }





    }






















}

