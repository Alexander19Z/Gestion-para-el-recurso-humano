using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Web;
using System.IO;

namespace CapaNegocio
{
    public class CN_Recursos
    {
        public static string generarClave()
        {
            /* guid genera un codigo unico N solo numero sub del 0 al 6*/
            string clave = Guid.NewGuid().ToString("N").Substring(0, 12);
            return clave;

        }


        /*Encryptado*/

        public static string ConvertirSha256(string texto)
        {
            //using System.Text;
            //USAR LA REFERENCIA DE "System.Security.Cryptography"

            StringBuilder Sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));

                foreach (byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString().ToUpper();
        }

        public static bool enviarCorreo(string correo, string asunto, string mensaje)
        {
            bool resultado = false;
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(correo);
                mail.From = new MailAddress("alexzun868@gmail.com");
                mail.Subject = asunto;
                mail.Body = mensaje;
                mail.IsBodyHtml = true;

                var smtp = new SmtpClient()
                {
                    Credentials = new NetworkCredential("alexzun868@gmail.com", "rltxkvzvjxkpdmgi"),
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true

                };




                smtp.Send(mail);
                resultado = true;


            }
            catch (Exception ex)
            {

                resultado = false;
            }
            return resultado;
        }
        public static bool enviarCorreocconAdjunto(string correo, string asunto, string mensaje, HttpPostedFileBase documento)
        {
            bool resultado = false;
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(correo);
                mail.From = new MailAddress("alexzun868@gmail.com");
                mail.Subject = asunto;
                mail.Body = mensaje;
                mail.IsBodyHtml = true;
              
                if (documento != null && documento.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(documento.FileName);
                    Attachment attachment = new Attachment(documento.InputStream, fileName);
                    mail.Attachments.Add(attachment);
                }

                var smtp = new SmtpClient()
                {
                    Credentials = new NetworkCredential("alexzun868@gmail.com", "rltxkvzvjxkpdmgi"),
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true

                };




                smtp.Send(mail);
                resultado = true;


            }
            catch (Exception ex)
            {

                resultado = false;
            }
            return resultado;
        }



    }
}
