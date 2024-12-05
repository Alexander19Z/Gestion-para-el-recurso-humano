using CapaEntidad;
using CapaNegocio;
using iTextSharp.text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace CapaPresentacionAdmin.Controllers
{
    public class NotificacionesController : Controller
    {
        // GET: Notificaciones
        public ActionResult Notificaciones()
        {
            if (Session["UsuarioLogueado"] != null)
            {
                persona oPersona = new persona();
                oPersona = Session["UsuarioLogueado"] as persona;

            }
            else
            {
                return RedirectToAction("Login", "Login");
            }

            return View();
        }
        public JsonResult listar_Notificaciones()
        {

            List<notificaciones> olista = new List<notificaciones>();
            olista = new CN_Notificacion().listar();

            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getPersona()
        {

            List<persona> olista = new List<persona>();
            olista = new CN_Persona().listar();
            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult guardarNotificaciones(notificaciones obj)
        {

            object resultado;
            string mensaje = string.Empty;

            if (obj.id_Noti == 0)
            {
                resultado = new CN_Notificacion().registrar(obj, out mensaje);

            }
            else
            {
                resultado = new CN_Notificacion().editar(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }




        [HttpPost]
        public JsonResult eliminarNotificaciones(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;
            respuesta = new CN_Notificacion().eliminar(id, out mensaje);
            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }






        public JsonResult ObtenerNotificaciones()
        {
            string mensaje = "";
            try
            {
                // Código para obtener las notificaciones
                // ...
                 persona oPersona = Session["UsuarioLogueado"] as persona;

                if (oPersona != null)
                {
                    List<listarSolicitudes> notificaciones = new List<listarSolicitudes>();
                    if (oPersona.tipo_Usuario.ToLower() == "lider")
                    {
                        notificaciones = new CN_Notificacion().listarSolicitudes(oPersona.id_Departamento);
                    }
                    else if (oPersona.tipo_Usuario.ToLower() == "recursos humanos")
                    {
                        notificaciones = new CN_Notificacion().listarSolicitudesRRHH();

                    }
                    else if (oPersona.tipo_Usuario.ToLower() == "gerente")
                    {
                        notificaciones = new CN_Notificacion().listarSolicitudesGerente();

                    }
                   

                    return Json(new { success = true, resultado = notificaciones, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
                }
                else {
                    return Json(new { success = false, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
                }

               
                
            }
            catch (Exception ex)
            {
                // Registra la excepción para su análisis posterior
                Console.WriteLine("Error al obtener notificaciones:", ex);

                // Devuelve un mensaje de error más informativo al cliente
                return Json(new { success = false, message = "Ocurrió un error al obtener las notificaciones." });
            }
        }






    }










}
