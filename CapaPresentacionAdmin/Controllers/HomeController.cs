using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacionAdmin.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            try
            {

                if (Session["UsuarioLogueado"] != null)
                   {
                      persona oPersona = Session["UsuarioLogueado"] as persona;
                    //Para buscar el nombre del departamento
                    List<departamento> olista = new List<departamento>();
                    olista = new CN_Departamento().listar();
                    var departamento = olista.FirstOrDefault(n => n.id_Departamento == oPersona.id_Departamento);

                    //Para buscar el puesto nombre
                    List<puesto> plista = new List<puesto>();
                    plista = new CN_Puesto().listar();
                    var puesto = plista.FirstOrDefault(n => n.id_Puesto == oPersona.id_Puesto);

                    //Para buscar notificaciones pendientes
                    List<notificaciones> nlista = new List<notificaciones>();
                    nlista = new CN_Notificacion().listar();
                    var notificaciones = nlista.Where(n => n.id_Persona == oPersona.id_Persona).ToList();
                    Session["Notificaciones"] = notificaciones;
                    Session["NombreCompleto"] = oPersona.nombre + " " + oPersona.apellido;
                    Session["Departamento"] = departamento.descripcion;
                    Session["Puesto"] = puesto.descripcion;


                }
                else
                {
                  return RedirectToAction("Login", "Login");
                }

                return View();




            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}