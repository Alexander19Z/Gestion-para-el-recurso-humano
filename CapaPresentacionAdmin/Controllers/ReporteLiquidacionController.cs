using CapaEntidad;
using CapaNegocio;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacionAdmin.Controllers
{
    public class ReporteLiquidacionController : Controller
    {
        // GET: ReporteLiquidacion
        public ActionResult reporteLiquidacion()
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


        public JsonResult listar_ReportesLiquidacion(string fecha)
        {
            try
            {
                List<reporteLiquidacion> olista = new CN_ReporteLiquidacion().listar(fecha);

                // Retornar la lista en el formato esperado por DataTables
                return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message, data = new List<reporteLiquidacion>() }, JsonRequestBehavior.AllowGet);
            }
        }





        public ActionResult DescargarReporteLiquidacion(string fecha)
        {
            try
            {
                List<reporteLiquidacion> listaLiquidacion = new CN_ReporteLiquidacion().listar(fecha);

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


                    Paragraph fechal = new Paragraph(DateTime.Now.ToString("dd/MM/yyyy"), FontFactory.GetFont(FontFactory.HELVETICA, 10));
                    fechal.Alignment = Element.ALIGN_CENTER;
                    doc.Add(fechal);


                    doc.Add(new Paragraph(" "));


                    Paragraph titulo = new Paragraph("Reporte ", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16));
                    titulo.Alignment = Element.ALIGN_CENTER;
                    doc.Add(titulo);

                    Paragraph subtitulo = new Paragraph($"Reporte de liquidación del mes de : {fecha}", FontFactory.GetFont(FontFactory.HELVETICA, 12));
                    subtitulo.Alignment = Element.ALIGN_CENTER;
                    doc.Add(subtitulo);

                    doc.Add(new Paragraph(" "));
                    Font font = new Font(Font.FontFamily.HELVETICA, 8);


                    PdfPTable table = new PdfPTable(10);
                    table.WidthPercentage = 100;


                    table.AddCell("Nombre");
                    table.AddCell("Apellido");
                    table.AddCell("Cédula");
                    table.AddCell("Fecha");
                    table.AddCell("Monto a pagar");
                    table.AddCell("Tipo de liquidación");
                    table.AddCell("Vacaciones no disfrutadas");
                    table.AddCell("Cesantía");
                    table.AddCell("Aguinaldo");
                    table.AddCell("Preaviso");

                    int totalPersonas = listaLiquidacion.Count;
                    decimal totalLiquidacion = 0m;

                    foreach (var usuario in listaLiquidacion)
                    {
                        table.AddCell(new Phrase(usuario.nombre, font));
                        table.AddCell(new Phrase(usuario.apellido, font));
                        table.AddCell(new Phrase(usuario.cedula, font));
                        table.AddCell(new Phrase(usuario.descripcion, font));
                        table.AddCell(new Phrase(usuario.monto_pagar.ToString(), font));
                        table.AddCell(new Phrase(usuario.tipo_Liquidacion, font));
                        table.AddCell(new Phrase("¢ " + usuario.vacacionesNoDistrutadas.ToString(), font));
                        table.AddCell(new Phrase("¢ " + usuario.cesantia.ToString(), font));
                        table.AddCell(new Phrase("¢ " + usuario.aguinaldo.ToString(), font));
                        table.AddCell(new Phrase("¢ " + usuario.preaviso.ToString(), font));
                        totalLiquidacion = totalLiquidacion + usuario.monto_pagar;
                    }

                    // Agregar una fila al final con el total de personas
                    PdfPCell cellTotal = new PdfPCell(new Phrase("Total de personas: " + totalPersonas));
                    cellTotal.Colspan = 10; 
                    cellTotal.HorizontalAlignment = Element.ALIGN_RIGHT; 
                    cellTotal.Border = Rectangle.TOP_BORDER; // 
                    table.AddCell(cellTotal);

                    PdfPCell cellTotalLiquidacion = new PdfPCell(new Phrase("Total de liquidación por pagar en el mes :¢ " + totalLiquidacion));
                    cellTotal.Colspan = 10; 
                    cellTotal.HorizontalAlignment = Element.ALIGN_RIGHT; 
                    cellTotal.Border = Rectangle.TOP_BORDER; 
                    table.AddCell(cellTotalLiquidacion);

                    doc.Add(table);
                    doc.Close();

                    // Descargar el PDF
                    byte[] byteArray = ms.ToArray();
                    return File(byteArray, "application/pdf", "ReporteLiquidación.pdf");
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