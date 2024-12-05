using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace CapaPresentacionAdmin.Controllers
{
    public class SolicitudPermisosController : Controller
    {
        // GET: SolicitudPermisos
        public ActionResult Permisos()
        {
           
            return View();
        }


        [HttpPost]
        public JsonResult SolicitarPermiso(permiso oPermiso)
        {
            try
            {
                notificaciones obj = new notificaciones();
                object resultado;
                string mensaje = "";
                persona oPersona = Session["UsuarioLogueado"] as persona;
                if (oPersona != null)
                {
                    DateTime horaInicio = DateTime.Parse(oPermiso.hora_Inicio);
                    DateTime horaFinalizacion = DateTime.Parse(oPermiso.hora_Finalizacion);

                    // Validaciones
                    bool esFinDeSemanaInicio = (horaInicio.DayOfWeek == DayOfWeek.Saturday || horaInicio.DayOfWeek == DayOfWeek.Sunday);
                    bool esFinDeSemanaFinalizacion = (horaFinalizacion.DayOfWeek == DayOfWeek.Saturday || horaFinalizacion.DayOfWeek == DayOfWeek.Sunday);
                    bool esAnteriorAHoyInicio = horaInicio < DateTime.Now;
                    bool esAnteriorAHoyFinalizacion = horaFinalizacion < DateTime.Now;

                    // Verificar horas laborales
                    bool horaInicioFueraDeRango = horaInicio.TimeOfDay < new TimeSpan(8, 0, 0) || horaInicio.TimeOfDay > new TimeSpan(17, 0, 0);
                    bool horaFinalizacionFueraDeRango = horaFinalizacion.TimeOfDay < new TimeSpan(8, 0, 0) || horaFinalizacion.TimeOfDay > new TimeSpan(17, 0, 0);

                    // Comprobar todas las condiciones en un solo if
                    if (esFinDeSemanaInicio || esFinDeSemanaFinalizacion ||
                        esAnteriorAHoyInicio || esAnteriorAHoyFinalizacion ||
                        horaInicioFueraDeRango || horaFinalizacionFueraDeRango)
                    {
                        return Json(new
                        {
                            success = false,
                            Message = "Por favor, verifica que los días seleccionados no sean fines de semana, no sean fechas anteriores a hoy, y que las horas estén entre las 8:00 y las 17:00."
                        });
                    }
                    else
                    {
                        // Guardar el permiso
                        permiso PermisoCompleto = new permiso
                        {
                            fecha_Solicitud = oPermiso.fecha_Solicitud,
                            hora_Inicio = horaInicio.ToString("yyyy-MM-ddTHH:mm"), // Guardar como texto
                            hora_Finalizacion = horaFinalizacion.ToString("yyyy-MM-ddTHH:mm"), // Guardar como texto
                            concepto = oPermiso.concepto,
                            descripcion = oPermiso.descripcion,
                            id_Persona = oPersona.id_Persona,
                        };

                        //Elegir sin son dias cuanto de le va rebar con goce de salario 
                        resultado = new CN_Permiso().registrar(PermisoCompleto, out mensaje);

                        if (resultado == null)
                        {
                            return Json(new { success = false , Message ="No se pudo registrar el permiso"});
                        }

                        DateTime fechaHoraInicio = DateTime.Parse(PermisoCompleto.hora_Inicio);
                        DateTime fechaHoraFinalizacion = DateTime.Parse(PermisoCompleto.hora_Finalizacion);
                        TimeSpan diferenciaTiempo = fechaHoraFinalizacion - fechaHoraInicio;

                        decimal totalDias = Math.Round((decimal)diferenciaTiempo.TotalDays, 0); // Cantidad de días completos
                        decimal totalHoras = (decimal)diferenciaTiempo.TotalHours; // Cantidad total de horas



                        obj.tipo_Notificacion = "Solicitud de permiso";
                        obj.estado = false;
                        obj.id_Persona = oPersona.id_Persona;
                        obj.fechasSolicitadas = PermisoCompleto.fecha_Solicitud;
                        obj.dias_Solicitado = Convert.ToDecimal(totalDias);
                        obj.horas_solicitadas = Convert.ToDecimal(totalHoras);

                        resultado = new CN_Notificacion().solicitudPermiso(obj, oPersona, oPermiso, out mensaje);
                        if (resultado == null)
                        {
                            return Json(new { success = false, Message = "No se pudo enviar la notificación" });
                        }

                    }
                }
                else
                {
                    return Json(new { success = false });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, Message = ex.Message });
            }

            return Json(new { success = true });
        }





    }
}