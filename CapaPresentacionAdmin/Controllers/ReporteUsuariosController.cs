using CapaEntidad;
using CapaNegocio;
using iTextSharp.text.pdf;
using iTextSharp.text;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacionAdmin.Controllers
{
    public class ReporteUsuariosController : Controller
    {
        // GET: ReporteUsuarios
        public ActionResult ReporteUsuarios()
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

        public JsonResult getDepartamento()
        {

            List<departamento> olista = new List<departamento>();
            olista = new CN_Departamento().listar();
            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult listar_ReportesUsuarios(string departamento)
        {
            try
            {
                List<reporteUsuarios> olista = new CN_reporteUsuario().listar(departamento);

                // Retornar la lista en el formato esperado por DataTables
                return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message, data = new List<reporteUsuarios>() }, JsonRequestBehavior.AllowGet);
            }
        }



  

 

        public ActionResult DescargarReporteUsuarios(string departamento)
            {
                try
                {
                    List<reporteUsuarios> listaUsuarios = new CN_reporteUsuario().listar(departamento);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        Document doc = new Document();
                        PdfWriter.GetInstance(doc, ms);
                        doc.Open();

                        
                        string imagePath = Server.MapPath("~/Content/DataTables/images/LogoSagicor.png"); 
                        if (System.IO.File.Exists(imagePath))
                        {
                            Image logo = Image.GetInstance(imagePath);
                            logo.Alignment = Element.ALIGN_CENTER;
                            logo.ScaleToFit(100f, 100f); 
                            doc.Add(logo);
                        }

                     
                        Paragraph fecha = new Paragraph(DateTime.Now.ToString("dd/MM/yyyy"), FontFactory.GetFont(FontFactory.HELVETICA, 10));
                        fecha.Alignment = Element.ALIGN_CENTER;
                        doc.Add(fecha);

                       
                        doc.Add(new Paragraph(" "));

                        
                        Paragraph titulo = new Paragraph("Reporte de Usuarios por Departamento", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16));
                        titulo.Alignment = Element.ALIGN_CENTER;
                        doc.Add(titulo);

                        Paragraph subtitulo = new Paragraph($"Departamento: {departamento}", FontFactory.GetFont(FontFactory.HELVETICA, 12));
                        subtitulo.Alignment = Element.ALIGN_CENTER;
                        doc.Add(subtitulo);

                        doc.Add(new Paragraph(" ")); 

                       
                        PdfPTable table = new PdfPTable(4);
                        table.WidthPercentage = 100;

                        
                        table.AddCell("Nombre");
                        table.AddCell("Apellido");
                        table.AddCell("Departamento");
                        table.AddCell("Puesto");

                        int totalPersonas = listaUsuarios.Count;

                       
                        foreach (var usuario in listaUsuarios)
                        {
                            table.AddCell(usuario.nombre);
                            table.AddCell(usuario.apellido);
                            table.AddCell(usuario.departamento);
                            table.AddCell(usuario.puesto);
                        }

                        // Agregar una fila al final con el total de personas
                        PdfPCell cellTotal = new PdfPCell(new Phrase("Total de personas: " + totalPersonas));
                        cellTotal.Colspan = 4; // La celda ocupa las cuatro columnas de la tabla
                        cellTotal.HorizontalAlignment = Element.ALIGN_RIGHT; // Alinear a la derecha
                        cellTotal.Border = Rectangle.TOP_BORDER; // Solo mostrar el borde superior para separar visualmente
                        table.AddCell(cellTotal);

                        doc.Add(table);
                        doc.Close();

                        // Descargar el PDF
                        byte[] byteArray = ms.ToArray();
                        return File(byteArray, "application/pdf", "ReporteUsuarios.pdf");
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de errores
                    return new HttpStatusCodeResult(500, ex.Message);
                }
        }

    }  
}