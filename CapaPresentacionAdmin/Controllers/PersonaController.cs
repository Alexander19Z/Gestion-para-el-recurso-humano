using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;

namespace CapaPresentacionAdmin.Controllers
{
    public class PersonaController : Controller
    {
        // GET: Persona
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult listar_Persona()
        {

            List<persona> olista = new List<persona>();
            olista = new CN_Persona().listar();

            return Json(new { data = olista },JsonRequestBehavior.AllowGet);    
        }

        public JsonResult getDepartamento()
        {

            List<departamento> olista = new List<departamento>();
            olista = new CN_Departamento().listar();
            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getPuesto()
        {

            List<puesto> olista = new List<puesto>();
            olista = new CN_Puesto().listar();
            //var nombrespuesto = olista.Select(d => new { d.descripcion }).ToList();
            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult guardarPersona( persona obj) {

            object resultado;
            string mensaje = string.Empty;
        
            if(obj.id_Persona== 0)
            {
                resultado= new CN_Persona().registrar(obj,out mensaje);

            }
            else
            {
                resultado= new CN_Persona().editar(obj,out mensaje) ;
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet); 

        }


        [HttpPost]
        public JsonResult eliminarPersona(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;
            respuesta = new CN_Persona().eliminar(id, out mensaje);
            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult restaurarCorreo(int id, string correoEmpresarial, string contrasena,string usuario)
        {
            bool respuesta = false;
            string mensaje = string.Empty;
            respuesta = new CN_Persona().restablecerCorreo(id,correoEmpresarial,contrasena,usuario, out mensaje);
            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


    }
}
