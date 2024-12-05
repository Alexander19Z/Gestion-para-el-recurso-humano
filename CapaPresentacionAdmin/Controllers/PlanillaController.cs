using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacionAdmin.Controllers
{
    public class PlanillaController : Controller
    {
        // GET: Planilla
        public ActionResult Planilla()
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

        public JsonResult listar_Planilla()
        {

            List<planilla> olista = new List<planilla>();
            olista = new CN_Planilla().listar();

            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }

     

        public JsonResult getPersona()
        {

            List<persona> olista = new List<persona>();
            olista = new CN_Persona().listar();
            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult guardarPlanilla(planilla obj)
        {

            object resultado;
            string mensaje = string.Empty;

            if (obj.id_Planilla == 0)
            {
                resultado = new CN_Planilla().registrar(obj, out mensaje);

            }
            else
            {
                resultado = new CN_Planilla().editar(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult eliminarPlanilla(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;
            respuesta = new CN_Planilla().eliminar(id, out mensaje);
            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }























    }
}