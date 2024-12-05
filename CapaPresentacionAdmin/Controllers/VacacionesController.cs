using CapaEntidad;
using CapaNegocio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace CapaPresentacionAdmin.Controllers
{
    public class VacacionesController : Controller
    {
        // GET: Vacaciones
        public ActionResult Vacaciones()
        {
            return View();
        }
        public JsonResult getPersona()
        {

            List<persona> olista = new List<persona>();
            olista = new CN_Persona().listar();
            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SolicitarVacaciones()
        {
            if(Session["UsuarioLogueado"] !=null) {
                persona oPersona = Session["UsuarioLogueado"] as  persona;
                List<vacaciones> vaca = new CN_Vacaciones().saldos(oPersona.id_Persona);
                var notificaciones = new CN_Notificacion()
             .listar()
             .Where(n => n.id_Persona == oPersona.id_Persona)
             .Select(n => n.fechasSolicitadas) 
             .ToList();

             var fechasFormateadas = notificaciones
            .Where(n => !string.IsNullOrWhiteSpace(n))
            .SelectMany(n => n.Split(','))
            .Select(f => f.Trim())
            .ToList();

                if (vaca != null && vaca.Count > 0)
                {
                    
                    TempData["DiaAcumulado"] = vaca[0].saldo_Vacaciones;
                    TempData["DiasSoli"] = vaca[0].dias_Solicitado;
                    TempData["Monto"] = vaca[0].monto_Total;
                }
                ViewBag.FechasSolicitadas = fechasFormateadas;
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
                                  

            return View();
        }


        public JsonResult listar_Vacaciones()
        {

            List<vacaciones> olista = new List<vacaciones>();
            olista = new CN_Vacaciones().listar();

            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult guardarVacaciones(vacaciones obj)
        {

            object resultado;
            string mensaje = string.Empty;

            if (obj.id_Persona == 0)
            {
                resultado = new CN_Vacaciones().registrar(obj, out mensaje);

            }
            else
            {
                resultado = new CN_Vacaciones().editar(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult eliminarVacaciones(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;
            respuesta = new CN_Vacaciones().eliminar(id, out mensaje);
            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SolicitarVacacione()
        {



            try
            {

             


             // Leer el cuerpo de la solicitud
                        string requestBody;
                        using (var reader = new StreamReader(Request.InputStream))
                        {
                            requestBody = reader.ReadToEnd();
                        }

                        // Deserializar los datos JSON
                        List<SelectedDay> selectedDays = JsonConvert.DeserializeObject<List<SelectedDay>>(requestBody);

                        decimal totalDiasSolicitado = 0.0m;
                        notificaciones obj = new notificaciones();
                        string fechas = null;
                        persona oPersona = null;
                        int resultado;
                        string mensaje = string.Empty;
                        decimal saldo_Vacaciones = 0.0m;

                        if (Session["UsuarioLogueado"] != null)
                        {
                            oPersona = Session["UsuarioLogueado"] as persona;

                         


                     List<vacaciones> vaca = new CN_Vacaciones().saldos(oPersona.id_Persona);

                            if (vaca != null && vaca.Count > 0)
                            {
                                TempData["DiaAcumulado"] = vaca[0].saldo_Vacaciones;
                                TempData["DiasSoli"] = vaca[0].dias_Solicitado;
                                TempData["Monto"] = vaca[0].monto_Total;
                                saldo_Vacaciones = vaca[0].saldo_Vacaciones;

                        if (selectedDays != null && selectedDays.Count > 0)
                        {
                            foreach (var day in selectedDays)
                            {
                                if (day.Type == "full")
                                {
                                    totalDiasSolicitado += 1;
                                    fechas = fechas + ", " + day.Date;
                                }
                                else
                                {
                                    totalDiasSolicitado += 0.5m;
                                    fechas = fechas + ", " + day.Date + " Medio día";
                                }

                            }
                            obj.tipo_Notificacion = "Solicitud de vacaciones";
                            obj.estado = false;
                            obj.id_Persona = oPersona.id_Persona;
                            obj.fechasSolicitadas = fechas;
                            obj.dias_Solicitado = totalDiasSolicitado;
                        }
                        else {




                            return Json(new { success = false, message = "Debe seleccionar una fecha!!" });

                        }



                                Boolean VerificacionNotificacionVaca = new CN_Vacaciones().VerificarNotificacion(oPersona.id_Persona, obj.tipo_Notificacion);
                                if (VerificacionNotificacionVaca == false) {
                                
                                    resultado = new CN_Notificacion().solicitudVacaciones(obj, saldo_Vacaciones, oPersona, out mensaje);

                                    if (resultado <= 0) {

                                         return Json(new { success = false,message ="No cuenta con días de vacaciones" });
                                      }
                                     
                                }
                                else {
                                        mensaje = "Actualmente cuenta con una solicitud activa. Espere a que sea aprobada o rechazada.";
                                return Json(new { success = false, message = mensaje });
                                }




                            }
                            else {

                                 return Json(new { success = false });

                              }
                        }
                         return Json(new { success = true });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Ocurrió un error"+ex }, JsonRequestBehavior.AllowGet);


            }
           
        }


        public class SelectedDay
        {
            public string Date { get; set; }
            public string Type { get; set; }
        }


      


    }
}