using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacionAdmin.Controllers
{
    public class LiquidacionController : Controller
    {
        // GET: Liquidacion
        public ActionResult Liquidacion()
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


        public JsonResult listar_Liquidacion()
        {

            List<liquidacion> olista = new List<liquidacion>();
            olista = new CN_Liquidacion().listar();

            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult guardarLiquidacion(liquidacion obj)
        {

            object resultado;
            string mensaje = string.Empty;

            if (obj.id_Liquidacion == 0)
            {
                resultado = new CN_Liquidacion().registrar(obj, out mensaje);

            }
            else
            {
                resultado = new CN_Liquidacion().editar(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult eliminarLiquidacion(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;
            respuesta = new CN_Liquidacion().eliminar(id, out mensaje);
            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


    }
}