using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace CapaPresentacionAdmin.Controllers
{
    public class cambioContrasenaController : Controller
    {
       
        // GET: cambioContrasena
        public ActionResult cambioContrasena()
        {
            

            if (Session["UsuarioLogueado"] != null)
            {
                persona oPersona = new persona();
                oPersona = Session["UsuarioLogueado"] as persona;
                ViewBag.NombreUsuario = oPersona.nombre + " " + oPersona.apellido;
                ViewBag.IdUsuario = oPersona.id_Persona;




            }
            else
            {
                return RedirectToAction("Login", "Login");
            }

            return View();

        }

        [HttpPost]
        public JsonResult cambioContrasena(string id_Persona, string oldPassword, string newPassword)
        {
            var res = false;
            var contraNuevaSha = string.Empty;
            var mensaje = string.Empty;
            try
            {
                persona oPersona = new persona();
                oPersona = new CN_Persona().listar().Where(u => u.id_Persona == Convert.ToInt32(id_Persona)  && u.contrasena == CN_Recursos.ConvertirSha256(oldPassword)).FirstOrDefault();


                if (CN_Recursos.ConvertirSha256(oldPassword) == oPersona.contrasena) { 
                
                    contraNuevaSha=CN_Recursos.ConvertirSha256(newPassword);
                    
                    res= new CN_Persona().cambiarClave(oPersona.id_Persona,contraNuevaSha,out mensaje);

                }else
                {
                    res = false;
                }






                // Aquí iría la lógica para cambiar la contraseña
                return Json(new { resultado = res, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex )
            {
                return Json(new { resultado = false, mensaje = ex }, JsonRequestBehavior.AllowGet);
            }
        }








    }
}