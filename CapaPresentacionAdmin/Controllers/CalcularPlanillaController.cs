using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using System.IO;
namespace CapaPresentacionAdmin.Controllers
{
    public class CalcularPlanillaController : Controller
    {
        // GET: CalcularPlanilla
        public ActionResult CalcularPlanilla()
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

        public JsonResult listar_Planilla()
        {
          
            List<planilla> olista = new CN_Planilla().listar();
        
            int mesActual = DateTime.Now.Month;
            int anoActual = DateTime.Now.Year;
            var planillasFiltradas = FiltrarPlanillasPorFecha(olista, mesActual, anoActual);

            return Json(new { data = planillasFiltradas }, JsonRequestBehavior.AllowGet);
   
        }


        public JsonResult calculoPlanilla()
        {
            var resultado = new CN_Planilla().calculoPlanilla();
            List<planilla> planilaCal = new CN_Planilla().listar();

            if (planilaCal.Count > 0)
            {


                // Obtener el mes y año actuales
                int mesActual = DateTime.Now.Month;
                int anoActual = DateTime.Now.Year;
                var planillasFiltradas = FiltrarPlanillasPorFecha(planilaCal, mesActual, anoActual);


                planillasFiltradas = new CN_Planilla().calcularRebajos(planillasFiltradas);
                planillasFiltradas = FiltrarPlanillasPorFecha(planilaCal, mesActual, anoActual);
                return Json(new { data = resultado }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = resultado }, JsonRequestBehavior.AllowGet);
            }




        }
        public ActionResult GuardarArchivo()
        {
         
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; 

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Datos");

                // Agregar el título
                worksheet.Cells[1, 1].Value = "Cálculo de planilla";
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
                worksheet.Cells[5, 1].Value = "ID Planilla";
                worksheet.Cells[5, 2].Value = "ID Persona";
                worksheet.Cells[5, 3].Value = "Salario Total";
                worksheet.Cells[5, 4].Value = "Deducción CCSS";
                worksheet.Cells[5, 5].Value = "Deducción Permiso";
                worksheet.Cells[5, 6].Value = "Deducción Impuesto Renta";
                worksheet.Cells[5, 7].Value = "Deducción Incapacidad";
                
                worksheet.Cells[5, 8].Value = "Deducción Varias";
                worksheet.Cells[5, 9].Value = "Salario a Pagar";
                worksheet.Cells[5, 10].Value = "Fecha Creación";
                worksheet.Cells[5, 11].Value = "Fecha Pago";

                List<planilla> olista = new CN_Planilla().listar();
                int mesActual = DateTime.Now.Month;
                int anoActual = DateTime.Now.Year;
                var planillasFiltradas = FiltrarPlanillasPorFecha(olista, mesActual, anoActual);

                for (int i = 0; i < planillasFiltradas.Count; i++)
                {
                    var planilla = planillasFiltradas[i];
                    worksheet.Cells[i + 6, 1].Value = planilla.id_Planilla;
                    worksheet.Cells[i + 6, 2].Value = planilla.id_Persona;
                    worksheet.Cells[i + 6, 3].Value = planilla.salario_Total;
                    worksheet.Cells[i + 6, 4].Value = planilla.deduccion_CCSS;
                    worksheet.Cells[i + 6, 5].Value = planilla.deduccion_permiso;
                    worksheet.Cells[i + 6, 6].Value = planilla.deduccion_InpuestoRenta;
                    worksheet.Cells[i + 6, 7].Value = planilla.deduccion_Incapacidad;
    
                    worksheet.Cells[i + 6, 8].Value = planilla.deducciones_Varias;
                    worksheet.Cells[i + 6, 9].Value = planilla.salario_Pagar;
                    worksheet.Cells[i + 6, 10].Value = planilla.fecha_creacion;
                    worksheet.Cells[i + 6, 11].Value = planilla.fecha_Pago;
                }

                worksheet.Cells[5, 1, 5, 13].Style.Font.Bold = true; // Encabezados en negrita
                worksheet.Cells[5, 1, 5, 13].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[5, 1, 5, 13].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

                worksheet.Cells.AutoFitColumns();
                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0; 

                var fileName = "CalculoPlanilla.xlsx"; 
                Response.Headers.Add("Content-Disposition", $"attachment; filename={fileName}"); 
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }

         
        }




        public List<planilla> FiltrarPlanillasPorFecha(List<planilla> olista, int mesActual, int añoActual)
        {
            var planillasFiltradas = olista.Where(p =>
            {
                DateTime fechaPago;
                // Si 'fecha_Pago' está en formato "MM/yyyy", intentamos convertirlo a DateTime.
                bool fechaValida = DateTime.TryParseExact(p.fecha_Pago, "yyyy-MM", null, System.Globalization.DateTimeStyles.None, out fechaPago);

                // Verificamos que la fecha sea válida y coincida con el mes y año actuales.
                return fechaValida && fechaPago.Month == mesActual && fechaPago.Year == añoActual;
            }).ToList();

            return planillasFiltradas;
        }
    }
}