using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacionAdmin.Controllers
{
    public class IncapacidadController : Controller
    {
        // GET: Incapacidad
        public ActionResult Incapacidad()
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



        public JsonResult listar_Incapacidad()
        {

            List<incapacidad> olista = new List<incapacidad>();
            olista = new CN_Incapacidad().listar();

            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult guardarIncapacidad(incapacidad obj)
        {

            object resultado;
            string mensaje = string.Empty;

            if (obj.id_Incapacidad == 0)
            {
                resultado = new CN_Incapacidad().registrar(obj, out mensaje);

            }
            else
            {
                resultado = new CN_Incapacidad().editar(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult eliminarIncapacidad(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;
            respuesta = new CN_Incapacidad().eliminar(id, out mensaje);
            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

















    }


}