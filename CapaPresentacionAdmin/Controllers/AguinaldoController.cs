using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacionAdmin.Controllers
{
    public class AguinaldoController : Controller
    {
        // GET: Aguinaldo
        public ActionResult Aguinaldo()
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

        public JsonResult listar_Aguinaldo()
        {

            List<aguinaldo> olista = new List<aguinaldo>();
            olista = new CN_Aguinaldo().listar();

            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult guardarAguinaldo(aguinaldo obj)
        {

            object resultado;
            string mensaje = string.Empty;

            if (obj.id_Aguinaldo == 0)
            {
                resultado = new CN_Aguinaldo().registrar(obj, out mensaje);

            }
            else
            {
                resultado = new CN_Aguinaldo().editar(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult eliminarAguinaldo(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;
            respuesta = new CN_Aguinaldo().eliminar(id, out mensaje);
            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }



    }
}