using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacionAdmin.Controllers
{
    public class ConsultaVacacionesController : Controller
    {
        // GET: ConsultaVacaciones
        public ActionResult ConsultaVacaciones()
        {

            if (Session["UsuarioLogueado"] != null)
            {
                persona oPersona = new persona();
                oPersona = Session["UsuarioLogueado"] as persona;
                ViewBag.NombreUsuario = oPersona.nombre + " " + oPersona.apellido;
                ViewBag.IdUsuario = oPersona.id_Persona;




            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
            return View();
           
        }
        [HttpPost]
        public JsonResult ConsultaVacaciones(int oPersona, string fecha,string tipoConsulta)
        {

            try
            {
                if (fecha!="") { 
                
                    List<notificaciones> noti = new CN_Notificacion().listar();

                    // Convertir la fecha recibida en formato "yyyy-MM" a un objeto DateTime
                    DateTime fechaConsulta = DateTime.ParseExact(fecha, "yyyy-MM", System.Globalization.CultureInfo.InvariantCulture);
                    int mesConsulta = fechaConsulta.Month;
                    int anioConsulta = fechaConsulta.Year;

           
                    var notificacionesFiltradas = noti
                        .Where(n => n.id_Persona == oPersona
                                    && n.tipo_Notificacion == tipoConsulta
                                    && n.fechasSolicitadas
                                        .Split(',')
                                        .Any(f => DateTime.TryParseExact(
                                                      f.Trim().Split(' ')[0], // Toma solo la parte de la fecha "dd/MM/yyyy"
                                                      "dd/MM/yyyy",
                                                      System.Globalization.CultureInfo.InvariantCulture,
                                                      System.Globalization.DateTimeStyles.None,
                                                      out DateTime fechaNoti)
                                                 && fechaNoti.Month == mesConsulta
                                                 && fechaNoti.Year == anioConsulta))
                        .ToList();

                    if (notificacionesFiltradas.Count == 0) {
                        return Json(new { data = notificacionesFiltradas,message="No hay resultados." }, JsonRequestBehavior.AllowGet);
                    }



                    return Json(new { data = notificacionesFiltradas }, JsonRequestBehavior.AllowGet);








                }
                else
                {

                    return Json(new {success=false, message = "Por favor, ingresa una fecha válida." }, JsonRequestBehavior.AllowGet);
                }
             
                



            }
            catch (Exception)
            {

                throw;
            }








        }







    }
}