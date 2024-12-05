using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacionAdmin.Controllers
{
    public class PermisoController : Controller
    {
        // GET: Permiso
        public ActionResult Permiso()
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





        public JsonResult listar_Permiso()
        {

            List<permiso> olista = new List<permiso>();
            olista = new CN_Permiso().listar();

            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult guardarPermiso(permiso obj)
        {

            object resultado;
            string mensaje = string.Empty;

            if (obj.id_Permiso == 0)
            {
                resultado = new CN_Permiso().registrar(obj, out mensaje);

            }
            else
            {
                resultado = new CN_Permiso().editar(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult eliminarPermiso(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;
            respuesta = new CN_Permiso().eliminar(id, out mensaje);
            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
    }
}