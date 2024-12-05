using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacionAdmin.Controllers
{
    public class ConsultaSalarioController : Controller
    {
        // GET: ConsultaSalario
        public ActionResult ConsultaSalario()
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
        public JsonResult ConsultaPlanilla(int oPersona, string fecha)
        {

            try
            {
                if (fecha != "")
                {
                    var resultado = new CN_Planilla().listar();
                    List<planilla> planilla = new CN_Planilla().listar();


                    DateTime fechaConsulta = DateTime.ParseExact(fecha, "yyyy-MM", System.Globalization.CultureInfo.InvariantCulture);
                    string fechaFormateada = fechaConsulta.ToString("yyyy-MM");

                    if (planilla.Count > 0)
                    {
                        var planillasFiltradas = planilla.FirstOrDefault(e => e.id_Persona == oPersona && e.fecha_Pago == fechaFormateada);

                        return Json(new { data = planillasFiltradas }, JsonRequestBehavior.AllowGet);


                    }
                    else
                    {
                        return Json(new { data = resultado }, JsonRequestBehavior.AllowGet);
                    }




                }
                else {
                    return Json(new { success = false, message = "Por favor, ingresa una fecha válida." }, JsonRequestBehavior.AllowGet);
                }

                       



            }
            catch (Exception)
            {

                return Json(new { success = false,message="Error en la carga de datos"  }, JsonRequestBehavior.AllowGet);
            }

           




        }












    }
}