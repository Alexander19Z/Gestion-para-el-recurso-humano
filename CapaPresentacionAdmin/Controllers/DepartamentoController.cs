using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacionAdmin.Controllers
{
    public class DepartamentoController : Controller
    {
        // GET: Departamento
        public ActionResult Departamento()
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




        public JsonResult listar_Departamento()
        {

            List<departamento> olista = new List<departamento>();
            olista = new CN_Departamento().listar();

            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult guardarDepartamento(departamento obj)
        {

            object resultado;
            string mensaje = string.Empty;

            if (obj.id_Departamento == 0)
            {
                resultado = new CN_Departamento().registrar(obj, out mensaje);

            }
            else
            {
                resultado = new CN_Departamento().editar(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult eliminarDepartamento(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;
            respuesta = new CN_Departamento().eliminar(id, out mensaje);
            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

















    }
}