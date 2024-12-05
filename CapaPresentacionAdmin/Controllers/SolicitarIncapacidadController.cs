using CapaEntidad;
using CapaNegocio;
using OfficeOpenXml.ConditionalFormatting.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Parser.SyntaxTree;
using System.Web.Services.Description;
using System.Web.UI.WebControls;

namespace CapaPresentacionAdmin.Controllers
{
    public class SolicitarIncapacidadController : Controller
    {
        // GET: SolicitarIncapacidad
        public ActionResult SolicitudIncapacidad()
        {
            if (Session["UsuarioLogueado"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            
            return View();
        }


        public JsonResult getPersona()
        {

            List<persona> olista = new List<persona>();
            olista = new CN_Persona().listar();
            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult soliIncapacidad(incapacidad oIncapacidad, HttpPostedFileBase documento)
        {
            try
            {
                if (oIncapacidad != null)
                {
                    // Validar fechas
                    DateTime fechaInicio;
                    DateTime fechaFinalizacion;
                    int diasHabilidos = 0;

                    if (!DateTime.TryParse(oIncapacidad.fecha_Inicio, out fechaInicio) ||
                        !DateTime.TryParse(oIncapacidad.fecha_Finalizacion, out fechaFinalizacion))
                    {
                        return Json(new { success = false, Message = "Por favor, verifica que las fechas sean válidas." });
                    }

                    // Verificar que las fechas no sean pasadas
                    if (fechaInicio < DateTime.Now || fechaFinalizacion < DateTime.Now )
                    {
                        return Json(new { success = false, Message = "Las fechas de inicio, finalización y solicitud no pueden ser anteriores a la fecha actual." });
                    }

                    
                    if (fechaInicio > fechaFinalizacion)
                    {
                        return Json(new { success = false, Message = "La fecha de inicio no puede ser posterior a la fecha de finalización." });
                    }

                    if (oIncapacidad.tipo_Incapacidad == null || oIncapacidad.tipo_Incapacidad == "null")
                    {
                        return Json(new { success = false, Message = "Debe elegir una incapacidad."});
                    }

                    if (fechaInicio.DayOfWeek == DayOfWeek.Saturday || fechaInicio.DayOfWeek == DayOfWeek.Sunday)
                    {
                        return Json(new { success = false, Message = "La fecha de inicio no puede ser un fin de semana." });
                    }

                    if (fechaFinalizacion.DayOfWeek == DayOfWeek.Saturday || fechaFinalizacion.DayOfWeek == DayOfWeek.Sunday)
                    {
                        return Json(new { success = false, Message = "La fecha de finalización no puede ser un fin de semana." });
                    }


                    if (documento == null)
                    {
                        return Json(new { success = false, Message = "El documento es obligatorio y no puede estar vacío." });
                    }
                    string extension = Path.GetExtension(documento.FileName).ToLower();
                    if (extension != ".pdf")
                    {
                        return Json(new { success = false, Message = "El documento debe ser un archivo PDF." });
                    }

                  
                    if (documento.ContentType != "application/pdf")
                    {
                        return Json(new { success = false, Message = "El documento debe ser un archivo PDF." });
                    }
                    persona per = Session["UsuarioLogueado"] as persona;
                    List<persona> pLista = new List<persona>(new CN_Persona().listar());
                    List<puesto> puLista = new List<puesto>(new CN_Puesto().listar());
                    decimal montonApagar;
                    string mensaje = string.Empty;
                    int result;
                    var motonTotal = 0m;
                    var totalDias = 0;

                   
                    var resultado = from p in pLista
                                    join pu in puLista on p.id_Puesto equals pu.id_Puesto
                                    select new
                                    {
                                        p.nombre,
                                        p.apellido,
                                        p.id_Persona,
                                        pu.salario_Bruto
                                    };

                    var listafiltro = resultado.Where(r => r.id_Persona == per.id_Persona).FirstOrDefault();
                    var salarioPromedioDiario = listafiltro.salario_Bruto / 30;
                    int diasHabilidosPorMes = 22; // Días hábiles en el mes
                    decimal totalPagarIncapacidad = 0;
                    switch (oIncapacidad.tipo_Incapacidad)
                    {
                        case "Enfermedad comun":
                            decimal salarioDiario = listafiltro.salario_Bruto / diasHabilidosPorMes;
                            
                           

                            
                            int diasNoHabilidos = 0;


                            for (DateTime fecha = fechaInicio; fecha <= fechaFinalizacion ; fecha = fecha.AddDays(1))
                            {

                                if (fecha.DayOfWeek != DayOfWeek.Saturday && fecha.DayOfWeek != DayOfWeek.Sunday)
                                {
                                    diasHabilidos++;
                                }
                                else
                                {
                                    diasNoHabilidos++;
                                }
                            }


                            if (diasHabilidos <= 3)
                            {
                                totalPagarIncapacidad = salarioDiario * diasHabilidos * 0.50m;
                            }
                            else
                            {
                                totalPagarIncapacidad = salarioDiario * 3 * 0.50m;
                            }

                           
                            break;

                        case "Licencias de maternidad":
                            var salarioMensual = listafiltro.salario_Bruto;
                            motonTotal = salarioMensual * 0.50m;
                            break;

                        case "Licencias de fase terminal":
                            diasHabilidos = CalcularDiasHabiles(fechaInicio, fechaFinalizacion);
                            totalPagarIncapacidad = (listafiltro.salario_Bruto / 30) * totalDias;
                            break;

                        case "Licencia para cuidado de persona menor gravemente enferma":
                            
                            diasHabilidos = CalcularDiasHabiles(fechaInicio, fechaFinalizacion);
                            totalPagarIncapacidad = (listafiltro.salario_Bruto / 30) * totalDias;
                            break;

                        case "Accidentes de tránsito":
                            
                            motonTotal = listafiltro.salario_Bruto / 3;
                            break;

                        case "Accidente laboral":

                            salarioDiario = listafiltro.salario_Bruto / diasHabilidosPorMes;
                            
                            diasHabilidos = 0;



                            for (DateTime fecha = fechaInicio; fecha <= fechaFinalizacion; fecha = fecha.AddDays(1))
                            {

                                if (fecha.DayOfWeek != DayOfWeek.Saturday && fecha.DayOfWeek != DayOfWeek.Sunday)
                                {
                                    diasHabilidos++;
                                }
                              
                            }


                            totalPagarIncapacidad = salarioDiario * diasHabilidos;
                            break;

                        case "Riesgo del trabajo":
                            totalPagarIncapacidad = salarioPromedioDiario * totalDias;
                            break;

                        default:
                            totalPagarIncapacidad = 0;
                            break;
                    }


                    var objIncapacidadNoti = new notificaciones
                    {
                        tipo_Notificacion = "Solicitud de incapacidad",
                        estado = false,
                        id_Persona = listafiltro.id_Persona,
                        fechasSolicitadas = Convert.ToDateTime(oIncapacidad.fecha_Inicio).ToString("dd/MM/yyyy") + "," + Convert.ToDateTime(oIncapacidad.fecha_Finalizacion).ToString("dd/MM/yyyy"),
                        dias_Solicitado = diasHabilidos,
                        horas_solicitadas = 0,
                        aprobacionRRHH = false
                    };

                    var resultadoNoti = new CN_Notificacion().registrar(objIncapacidadNoti, out mensaje);
                    var obj = new incapacidad
                    {
                        tipo_Incapacidad = oIncapacidad.tipo_Incapacidad,
                        fecha_Solicitud = oIncapacidad.fecha_Solicitud, // Formato correcto
                        fecha_Inicio = oIncapacidad.fecha_Inicio,
                        fecha_Finalizacion = oIncapacidad.fecha_Finalizacion,
                        monto_Apagar = totalPagarIncapacidad,
                        descripcion = oIncapacidad.descripcion,
                        id_Persona = listafiltro.id_Persona
                    };


                    result = new CN_Incapacidad().registrar(obj, out mensaje);
                    if (result > 0)
                    {
                        var resulCorreo = new CN_Notificacion().EnviarIncapacidad(per, obj, documento);
                    }

                    return Json(new { success = true, Message = mensaje }, JsonRequestBehavior.AllowGet);
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
        }



        public static int CalcularDiasHabiles(DateTime fechaInicio, DateTime fechaFinalizacion)
        {
            int totalDiasHabiles = 0;
            DateTime fechaActual = fechaInicio;

            while (fechaActual <= fechaFinalizacion)
            {
                // Verificar si el día no es sábado (6) ni domingo (0)
                if (fechaActual.DayOfWeek != DayOfWeek.Saturday && fechaActual.DayOfWeek != DayOfWeek.Sunday)
                {
                    totalDiasHabiles++;
                }

                // Avanzar al siguiente día
                fechaActual = fechaActual.AddDays(1);
            }

            return totalDiasHabiles;
        }




    }
}