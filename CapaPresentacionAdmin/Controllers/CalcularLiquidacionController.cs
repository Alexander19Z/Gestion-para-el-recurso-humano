using CapaEntidad;
using CapaNegocio;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacionAdmin.Controllers
{
    public class CalcularLiquidacionController : Controller
    {
        // GET: CalcularLiquidacion
        public ActionResult calcularLiquidacion()
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




        [HttpPost]
        public JsonResult calcularl(int idPersona, string tipoLiquidacion, string tipoPreaviso, DateTime fechaSalida)
        {
            var mensaje = string.Empty;
            var fechaActual = DateTime.Now;

            if (idPersona.ToString().Length < 12 && fechaSalida.Month == fechaActual.Month && fechaSalida.Year == fechaActual.Year)
            {
                var lista = new CN_Liquidacion().calcularLiquidacion(idPersona, tipoLiquidacion, tipoPreaviso, fechaSalida);

                if (lista.Count > 0)
                {
                    var pdfBytes = GenerarPdf(lista, idPersona); // Genera el PDF

                    string pdfUrl = SavePdfToServer(pdfBytes, $"Liquidacion_{idPersona}.pdf"); // Guarda el PDF y genera la URL

                    return Json(new { resultado = lista, mensaje = mensaje, pdfUrl = pdfUrl }, JsonRequestBehavior.AllowGet);
                }
                else {
                    return Json(new { resultado = lista, mensaje = "Usuario se encuentra inactivo."}, JsonRequestBehavior.AllowGet);
                }
              
            }
            else
            {
                mensaje = fechaSalida.Month != fechaActual.Month || fechaSalida.Year != fechaActual.Year
                    ? "La fecha de salida debe ser del mes actual."
                    : "El ID de persona no cumple con la longitud adecuada.";

                return Json(new { resultado = (object)null, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
            }
        }

        private string SavePdfToServer(byte[] pdfBytes, string fileName)
        {
            string filePath = Server.MapPath($"~/Content/PDFs/{fileName}");
            System.IO.File.WriteAllBytes(filePath, pdfBytes);
            return Url.Content($"~/Content/PDFs/{fileName}");
        }


        private byte[] GenerarPdf(IEnumerable<dynamic> lista, int idPersona)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document(PageSize.A4, 50, 50, 50, 50);
                PdfWriter.GetInstance(doc, ms);
                doc.Open();

                // Agregar logo
                string imagePath = Server.MapPath("~/Content/DataTables/images/LogoSagicor.png");
                if (System.IO.File.Exists(imagePath))
                {
                    Image logo = Image.GetInstance(imagePath);
                    logo.Alignment = Element.ALIGN_CENTER;
                    logo.ScaleToFit(120f, 120f);
                    doc.Add(logo);
                }

                // Fecha y título
                Paragraph fecha = new Paragraph(DateTime.Now.ToString("dd/MM/yyyy"), FontFactory.GetFont(FontFactory.HELVETICA, 10));
                fecha.Alignment = Element.ALIGN_RIGHT;
                doc.Add(fecha);

                doc.Add(new Paragraph(" ")); // Espacio
                Paragraph titulo = new Paragraph("Desglose de Liquidación", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16));
                titulo.Alignment = Element.ALIGN_CENTER;
                doc.Add(titulo);
                doc.Add(new Paragraph(" ")); // Espacio

                // Información del empleado
                Paragraph empleadoInfo = new Paragraph($"Cédula del empleado: {idPersona}", FontFactory.GetFont(FontFactory.HELVETICA, 12));
                doc.Add(empleadoInfo);
                doc.Add(new Paragraph(" ")); // Espacio

                // Desglose detallado
                foreach (var item in lista)
                {
                    doc.Add(new Paragraph($"Vacaciones no disfrutadas: ¢ {item.vacacionesNoDistrutadas:N2}", FontFactory.GetFont(FontFactory.HELVETICA, 12)));
                    doc.Add(new Paragraph($"Aguinaldo: ¢ {item.aguinaldo:N2}", FontFactory.GetFont(FontFactory.HELVETICA, 12)));
                    doc.Add(new Paragraph($"Preaviso: ¢ {item.preaviso:N2}", FontFactory.GetFont(FontFactory.HELVETICA, 12)));
                    doc.Add(new Paragraph($"Cesantía: ¢ {item.cesantia:N2}", FontFactory.GetFont(FontFactory.HELVETICA, 12)));
                    doc.Add(new Paragraph($"Salario pendiente: ¢ {item.diasTrabajados:N2}", FontFactory.GetFont(FontFactory.HELVETICA, 12)));
                    doc.Add(new Paragraph(" ")); // Espacio
                    doc.Add(new Paragraph($"Monto total a pagar: ¢ {item.monto_pagar:N2}", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14)));
                }

                doc.Close();
                return ms.ToArray();
            }
        }
















    }
}