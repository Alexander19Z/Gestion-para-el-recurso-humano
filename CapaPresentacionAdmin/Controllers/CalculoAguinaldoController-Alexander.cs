using CapaEntidad;
using CapaNegocio;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacionAdmin.Controllers
{
    public class CalculoAguinaldoController : Controller
    {
        // GET: CalculoAguinaldo
        public ActionResult CalculoAguinaldo()
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


     

        public JsonResult listarMeses()
        {
            try
            {
                List<aguinaldoMeses> olista = new List<aguinaldoMeses>();
                olista = new CN_Aguinaldo().listarMeses();
                if (olista != null) {

                    return Json(olista, JsonRequestBehavior.AllowGet);
                }else {

                    return Json(new { success = false, message ="Error en el cálculo del aguinaldo" }, JsonRequestBehavior.AllowGet);
                }
               

                
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }



        public ActionResult GuardarArchivoAguinaldo() {

            try
            {


                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Datos");

                    // Agregar el título
                    worksheet.Cells[1, 1].Value = "Cálculo de aguinaldo";
                    worksheet.Cells[1, 1, 1, 12].Merge = true;
                    worksheet.Cells[1, 1].Style.Font.Bold = true;
                    worksheet.Cells[1, 1].Style.Font.Size = 16;
                    worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    // Agregar el subtítulo
                    worksheet.Cells[2, 1].Value = "Asegurador Sagicor Costa Rica";
                    worksheet.Cells[2, 1, 2, 12].Merge = true;
                    worksheet.Cells[2, 1].Style.Font.Bold = true;
                    worksheet.Cells[2, 1].Style.Font.Size = 14;
                    worksheet.Cells[2, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    // Agregar la fecha actual
                    worksheet.Cells[3, 1].Value = $"Fecha: {DateTime.Now.ToShortDateString()}";
                    worksheet.Cells[3, 1, 3, 12].Merge = true;
                    worksheet.Cells[3, 1].Style.Font.Bold = true;
                    worksheet.Cells[3, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    // Agregar encabezados de columnas
                    worksheet.Cells[5, 1].Value = "ID Persona";
                    worksheet.Cells[5, 2].Value = "Diciembre anterior";
                    worksheet.Cells[5, 3].Value = "Enero";
                    worksheet.Cells[5, 4].Value = "Febrero";
                    worksheet.Cells[5, 5].Value = "Marzo";
                    worksheet.Cells[5, 6].Value = "Abril";
                    worksheet.Cells[5, 7].Value = "Mayo";
                    worksheet.Cells[5, 8].Value = "Junio";
                    worksheet.Cells[5, 9].Value = "Julio";
                    worksheet.Cells[5, 10].Value = "Agosto";
                    worksheet.Cells[5, 11].Value = "Septiembre";
                    worksheet.Cells[5, 12].Value = "Octubre";
                    worksheet.Cells[5, 13].Value = "Noviembre";
                    worksheet.Cells[5, 14].Value = "Total a pagar";
                    List<aguinaldoMeses> olista = new CN_Aguinaldo().listarMeses();
                    int mesActual = DateTime.Now.Month;
                    int anoActual = DateTime.Now.Year;
                    

                    for (int i = 0; i < olista.Count; i++)
                    {
                        var aguinaldo = olista[i];
                        worksheet.Cells[i + 6, 1].Value = aguinaldo.id_Persona;
                        worksheet.Cells[i + 6, 2].Value = aguinaldo.Diciembre_Anterior;
                        worksheet.Cells[i + 6, 3].Value = aguinaldo.Enero;
                        worksheet.Cells[i + 6, 4].Value = aguinaldo.Febrero;
                        worksheet.Cells[i + 6, 5].Value = aguinaldo.Marzo;
                        worksheet.Cells[i + 6, 6].Value = aguinaldo.Abril;
                        worksheet.Cells[i + 6, 7].Value = aguinaldo.Mayo;
                        worksheet.Cells[i + 6, 8].Value = aguinaldo.Junio;
                        worksheet.Cells[i + 6, 9].Value = aguinaldo.Julio;
                        worksheet.Cells[i + 6, 10].Value = aguinaldo.Agosto;
                        worksheet.Cells[i + 6, 11].Value = aguinaldo.Septiembre;
                        worksheet.Cells[i + 6, 12].Value = aguinaldo.Octubre;
                        worksheet.Cells[i + 6, 13].Value = aguinaldo.Noviembre;
                        worksheet.Cells[i + 6, 14].Value = aguinaldo.aguinaldoPagar;
                    }

                    worksheet.Cells[5, 1, 5, 14].Style.Font.Bold = true; // Encabezados en negrita
                    worksheet.Cells[5, 1, 5, 14].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[5, 1, 5, 14].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

                    worksheet.Cells.AutoFitColumns();
                    var stream = new MemoryStream();
                    package.SaveAs(stream);
                    stream.Position = 0;

                    var fileName = "CalculoAguinaldo.xlsx";
                    Response.Headers.Add("Content-Disposition", $"attachment; filename={fileName}");
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                }



            }
            catch (Exception)
            {

                throw;
            }
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        }




    }
}