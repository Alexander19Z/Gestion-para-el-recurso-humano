using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacionAdmin.Controllers
{
    public class PuestoController : Controller
    {
        // GET: Puesto
        public ActionResult Puesto()
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


        public JsonResult listar_Puesto()
        {

            List<puesto> olista = new List<puesto>();
            olista = new CN_Puesto().listar();

            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult guardarPuesto(puesto obj)
        {

            object resultado;
            string mensaje = string.Empty;

            if (obj.id_Puesto == 0)
            {
                resultado = new CN_Puesto().registrar(obj, out mensaje);

            }
            else
            {
                resultado = new CN_Puesto().editar(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult eliminarPuesto(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;
            respuesta = new CN_Puesto().eliminar(id, out mensaje);
            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
    }
}
