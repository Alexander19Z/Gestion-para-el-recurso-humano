using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;

namespace CapaPresentacionAdmin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
          

            return View();
        }

        public ActionResult RestablecerContraseña()
        {
            return View();
        }




        [HttpPost]
        public ActionResult Login(string usuario, string clave)
        {
            persona oPersona = new persona();
            oPersona = new CN_Persona().listar().Where(u => u.usuario.ToLower() == usuario.ToLower() && u.contrasena == CN_Recursos.ConvertirSha256(clave)).FirstOrDefault();

            if(oPersona == null)
            {
                ViewBag.Error = "Usuario o contraseña incorrecta; contacte al administrador del sistema.";
                return View();
            }
            else
            {
                ViewBag.Error = null;
                Session["UsuarioLogueado"] = oPersona;
                return RedirectToAction("Index", "Home");
            }


            
        }

         [HttpPost]
        public ActionResult RestablecerContraseña(string correo)
        {
            persona oPersona = new persona();
            oPersona = new CN_Persona().listar().Where(u => u.correo_Empresarial == correo ).FirstOrDefault();

            if (oPersona == null)
            {
                ViewBag.Error = "El correo electrónico no está en nuestra base de datos. Asegúrate de haber escrito el correo correctamente.";
                return View();
            }
            else
            {
                ViewBag.Error = null;
                    bool respuesta = false;
                    string mensaje = string.Empty;
                    respuesta = new CN_Persona().restablecerCorreo(oPersona.id_Persona, oPersona.correo_Empresarial,oPersona.contrasena,oPersona.usuario, out mensaje);

                    
                return RedirectToAction("Login", "Login");
            }

        }

    }

}