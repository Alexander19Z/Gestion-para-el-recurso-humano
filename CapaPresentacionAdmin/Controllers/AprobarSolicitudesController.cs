using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacionAdmin.Controllers
{
    public class AprobarSolicitudesController : Controller
    {
        // GET: AprobarSolicitudes
        public ActionResult AprobarSolicitudes()
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
            try
            {
                persona oPersona = Session["UsuarioLogueado"] as persona; 
                List<listarSolicitudes> olista = new List<listarSolicitudes>();
                
                if (oPersona != null)
                {
                    if (oPersona.tipo_Usuario.ToLower()=="lider") {
                    
                      olista = new CN_Notificacion().listarSolicitudes(oPersona.id_Departamento);
                    }else if (oPersona.tipo_Usuario.ToLower() =="recursos humanos") {
                        olista = new CN_Notificacion().listarSolicitudesRRHH();
                    
                    }
                    else if (oPersona.tipo_Usuario.ToLower() == "gerente")
                    {
                        olista = new CN_Notificacion().listarSolicitudesGerente();

                    }



                    return Json(new { data = olista }, JsonRequestBehavior.AllowGet);

                }
                return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                List<listarSolicitudes> olista = new List<listarSolicitudes>();
                olista = null;
                return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
            }
            
        }




        [HttpPost]
        public JsonResult guardarNotificaciones(listarSolicitudes noti)
        {
            bool resultado = false;
            int result;
            int resulVaca = 0;
            string mensaje = string.Empty;
            persona oPersona = Session["UsuarioLogueado"] as persona;


            try
            {

                if (oPersona.tipo_Usuario.ToLower() == "lider")
                {

                    if (noti.tipo_Notificacion.ToLower() == "solicitud de vacaciones")
                    {
                        resultado = new CN_Notificacion().actualizarNotificacion(noti.id_Noti, 1, 0);
                        resulVaca = new CN_Vacaciones().actualizarVacaciones(noti.nombre, noti.dias_Solicitado);
                        if (resultado == true && resulVaca >= 1)
                        {
                            mensaje = "El registro ha sido aprobado exitosamente.";
                            //Guardar el registro de la vacaciones solicitadas
                            DateTime fec = DateTime.Now;
                            registro_Vaca registro_Vaca = new registro_Vaca
                            {
                                tipo_Registro = '-',
                                id_Persona = noti.id_Persona,
                                fecha_Registro = fec.ToString("yyyy-M-d"),
                                tipo_Movimiento = "Solicitud de vacaciones",
                                periodo = DateTime.Now.Year.ToString(),



                            };
                            var reEnvioRRhh = new CN_Notificacion().solicitudVacacionesRRHH(noti, oPersona, out mensaje);
                            if (reEnvioRRhh == true)
                            {
                                result = new CN_RegistroVaca().registrar(registro_Vaca, out mensaje);
                                resultado = true;
                            }

                        }
                        else
                        {
                            mensaje = "Hubo un problema al aprobar el registro.";
                            return Json(new { resultado = false, mensaje = mensaje });
                        }
                    }
                    else
                    {
                        var reEnvioRRhh = new CN_Notificacion().solicitudVacacionesRRHH(noti, oPersona, out mensaje);
                        resultado = new CN_Notificacion().actualizarNotificacion(noti.id_Noti, 1, 0);
                     
                        resultado = reEnvioRRhh;
                    }

                }
                else if (oPersona.tipo_Usuario.ToLower() == "recursos humanos")
                {

                    resultado = new CN_Notificacion().actualizarNotificacion(noti.id_Noti, 1, 1);
                    if (resultado)
                    {
                        var aprovaciones = new CN_Notificacion().solicitudAprobadaRRHH(noti, oPersona, out mensaje);
                        resultado = aprovaciones;

                    }
                    if(noti.tipo_Notificacion.ToLower()=="solicitud de incapacidad") { resultado = new CN_Incapacidad().aprobacion(noti.id_Persona); }

                }
                else if (oPersona.tipo_Usuario.ToLower() == "gerente") {

                    if (noti.tipo_Notificacion.ToLower() == "solicitud de vacaciones")
                    {
                        resultado = new CN_Notificacion().actualizarNotificacion(noti.id_Noti, 1, 0);
                        resulVaca = new CN_Vacaciones().actualizarVacaciones(noti.nombre, noti.dias_Solicitado);
                        if (resultado == true && resulVaca >= 1)
                        {
                            mensaje = "El registro ha sido aprobado exitosamente.";
                            //Guardar el registro de la vacaciones solicitadas
                            DateTime fec = DateTime.Now;
                            registro_Vaca registro_Vaca = new registro_Vaca
                            {
                                tipo_Registro = '-',
                                id_Persona = noti.id_Persona,
                                fecha_Registro = fec.ToString("yyyy-M-d"),
                                tipo_Movimiento = "Solicitud de vacaciones",
                                periodo = DateTime.Now.Year.ToString(),



                            };
                            
                      
                                result = new CN_RegistroVaca().registrar(registro_Vaca, out mensaje);
                                resultado = true;
                            

                        }
                        else
                        {
                            mensaje = "Hubo un problema al aprobar el registro.";
                            return Json(new { resultado = false, mensaje = mensaje });
                        }
                    }
                    else
                    {
                        
                        resultado = new CN_Notificacion().actualizarNotificacion(noti.id_Noti, 1, 0);
                        
                    }



                    resultado = new CN_Notificacion().actualizarNotificacion(noti.id_Noti, 1, 1);
                    if (resultado)
                    {
                        var aprovaciones = new CN_Notificacion().solicitudAprobadaRRHH(noti, oPersona, out mensaje);
                        resultado = aprovaciones;

                    }









                }
               
               
            }
            catch (Exception ex)
            {
                // Log the exception details
                mensaje = "Ocurrió un error: " + ex.Message;
            }

            return Json(new { resultado = resultado, mensaje = mensaje });
        }

        [HttpPost]
        public JsonResult eliminarSolicitud(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;
            respuesta = new CN_Notificacion().eliminarSolicitud(id, out mensaje);
            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

    }
}