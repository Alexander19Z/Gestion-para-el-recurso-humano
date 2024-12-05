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
    public class ReporteRegistroVacaController : Controller
    {
        // GET: ReporteRegistroVaca
        public ActionResult ReporteRegistroVaca()
        {
           

            return View();
        }




        public JsonResult listar_ReportesRegistroVaca(string id_persona)
        {
            try
            {
                int persona = 0;
                if (id_persona.ToLower() == "Todos".ToLower())
                {
                    persona = 0;
                }
                else { 
                    persona = Convert.ToInt32(id_persona);
                }

                List<registro_Vaca> olista = new CN_RegistroVaca().ReporteListar(persona);

                // Retornar la lista en el formato esperado por DataTables
                return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message, data = new List<reporteLiquidacion>() }, JsonRequestBehavior.AllowGet);
            }
        }


             public ActionResult DescargarReporteRegistroVacas(string id_persona)
                {
                    try
                    {

                        int persona = 0;
                        if (id_persona.ToLower() == "Todos".ToLower())
                        {
                            persona = 0;
                        }
                        else
                        {
                            persona = Convert.ToInt32(id_persona);
                        }

                List<registro_Vaca> listaRegistroVaca = new CN_RegistroVaca().ReporteListar(persona);

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


                            Paragraph titulo = new Paragraph("Reporte de registro de vacaciones", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16));
                            titulo.Alignment = Element.ALIGN_CENTER;
                            doc.Add(titulo);

                            

                            doc.Add(new Paragraph(" "));
                            Font font = new Font(Font.FontFamily.HELVETICA, 6);


                            PdfPTable table = new PdfPTable(6);
                            table.WidthPercentage = 100;


                            table.AddCell("ID registro");
                            table.AddCell("tipo_Registro");
                            table.AddCell("Persona");
                            table.AddCell("Fecha registro");
                            table.AddCell("Tipo movimiento");
                            table.AddCell("Periodo");
                         

                       

                            foreach (var usuario in listaRegistroVaca)
                            {
                                table.AddCell(new Phrase(usuario.id_Registro.ToString(), font));
                                table.AddCell(new Phrase(usuario.tipo_Registro.ToString(), font));
                                table.AddCell(new Phrase(usuario.nombre+" "+usuario.apellido, font));
                                table.AddCell(new Phrase(usuario.fecha_Registro, font));
                                table.AddCell(new Phrase(usuario.tipo_Movimiento.ToString(), font));
                                table.AddCell(new Phrase(usuario.periodo.ToString(), font));
                                
                            }

                        

                          

                            doc.Add(table);
                            doc.Close();

                            // Descargar el PDF
                            byte[] byteArray = ms.ToArray();
                            return File(byteArray, "application/pdf", "ReporteRegistro.pdf");
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
