using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacionAdmin.Controllers
{
    public class DeduccionesController : Controller
    {
        // GET: Deducciones
        public ActionResult Deducciones()
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

        public JsonResult getPersona()
        {

            List<persona> olista = new List<persona>();
            olista = new CN_Persona().listar();
            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult listar_Deduccion()
        {

            List<deducciones> olista = new List<deducciones>();
            olista = new CN_Deducciones().listar();

            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult guardarDeducciones(deducciones obj)
        {

            object resultado;
            string mensaje = string.Empty;

            if (obj.id_Deduccion == 0)
            {
                resultado = new CN_Deducciones().registrar(obj, out mensaje);

            }
            else
            {
                resultado = new CN_Deducciones().editar(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult eliminarDeduccion(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;
            respuesta = new CN_Deducciones().eliminar(id, out mensaje);
            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }








    }
}