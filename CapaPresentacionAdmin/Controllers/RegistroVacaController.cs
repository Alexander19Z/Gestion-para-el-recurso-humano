using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacionAdmin.Controllers
{
    public class RegistroVacaController : Controller
    {
        // GET: RegistroVaca
        public ActionResult RegistroVaca()
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


        public JsonResult listar_RegistroVaca()
        {

            List<registro_Vaca> olista = new List<registro_Vaca>();
            olista = new CN_RegistroVaca().listar();

            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult guardarRegistroVaca(registro_Vaca obj)
        {

            object resultado;
            string mensaje = string.Empty;

            if (obj.id_Registro == 0)
            {
                resultado = new CN_RegistroVaca().registrar(obj, out mensaje);

            }
            else
            {
                resultado = new CN_RegistroVaca().editar(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult eliminarRegistroVaca(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;
            respuesta = new CN_RegistroVaca().eliminar(id, out mensaje);
            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }



    }
}