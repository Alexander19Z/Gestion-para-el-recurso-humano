using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;
namespace CapaNegocio
{
    public class CN_Persona
    {
        private CD_Persona objCapaDato= new CD_Persona();
        private CD_Vacaciones objCapaDatoVaca = new CD_Vacaciones();

        public List<persona> listar()
        {
            return objCapaDato.listar();    
        }


        public int registrar(persona obj, out string mensaje)
        {
            int res = 0;
            //Validaciones
            mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.nombre) || string.IsNullOrWhiteSpace(obj.nombre))
            {
                mensaje = "El nombre no puede estar vacío.";
            }
         
            else if (string.IsNullOrEmpty(obj.apellido) || string.IsNullOrWhiteSpace(obj.apellido))
            {
                mensaje = "El apellido no puede estar vacío.";
            }
          
            else if (string.IsNullOrEmpty(obj.cedula) || string.IsNullOrWhiteSpace(obj.cedula) || !int.TryParse(obj.cedula, out _) || (obj.cedula.Length != 9 && obj.cedula.Length != 12))
            {
                mensaje = "La cédula debe tener exactamente 9 o 12 dígitos y no puede estar vacía.";
            }
          
            else if (string.IsNullOrEmpty(obj.correo_Empresarial) || !obj.correo_Empresarial.Contains("@"))
            {
                mensaje = "El correo empresarial no puede estar vacío y debe contener un '@'.";
            }

            if (string.IsNullOrEmpty(obj.provincia) || string.IsNullOrWhiteSpace(obj.provincia))
            {
                mensaje = "El campo provincia no puede estar vacío.";
            }

            if (string.IsNullOrEmpty(obj.canton) || string.IsNullOrWhiteSpace(obj.canton))
            {
                mensaje = "El campo cantón no puede estar vacío.";
            }

            if (string.IsNullOrEmpty(obj.distrito) || string.IsNullOrWhiteSpace(obj.distrito))
            {
                mensaje = "El campo ditrito no puede estar vacío.";
            }

            else if (string.IsNullOrEmpty(obj.usuario) || string.IsNullOrWhiteSpace(obj.usuario))
            {
                mensaje = "El usuario no puede estar vacío.";
            }
            
            else if (obj.cantidad_Hijos.ToString() == null || obj.cantidad_Hijos < 0)
            {
                mensaje = "La cantidad de hijos no puede estar vacía o ser negativa.";
            }

            //creacion de clave automatica
            if (string.IsNullOrEmpty(mensaje))

            {
                {  
                    string clave = CN_Recursos.generarClave();
                    if (clave != null)
                    {
                        obj.contrasena = CN_Recursos.ConvertirSha256(clave);
                        res = objCapaDato.registrar(obj, out mensaje);
                        if (res != 0)
                        {
                            string asunto = "Creacion de cuenta";
                            string mensaje_correo = "<body style=\"text-align: center; font-family: Arial, sans-serif;\">\r\n    <div style=\"margin-top: 50px;\">\r\n             <h2 style=\"margin-top: 20px;\">Contraseña nueva</h2>\r\n        <p>Estimado Usuario,</p>\r\n        <p>Se ha solicitado su usuario y contraseña nueva.</p>\r\n     <p>Su usuario es:</p>\r\n        <p style=\"font-size: 24px; font-weight: bold;\">!usuario!</p>\r\n   <p>Su nueva contraseña es:</p>\r\n        <p style=\"font-size: 24px; font-weight: bold;\">!clave!</p>\r\n        <p>Por favor, asegúrese de cambiar esta contraseña después de iniciar sesión.</p>\r\n        <p>Gracias,<br>El equipo de Sagicor</p>\r\n    </div>\r\n</body>";
                            mensaje_correo = mensaje_correo.Replace("!clave!",clave);
                            mensaje_correo = mensaje_correo.Replace("!usuario!", obj.usuario);

                            bool respuesta = CN_Recursos.enviarCorreo(obj.correo_Empresarial, asunto, mensaje_correo);
                            if (respuesta)
                            {
                               
                                vacaciones obj2 = new vacaciones
                                {
                                    id_Persona=res,
                                    saldo_Vacaciones=0,
                                    dias_Ganado=0,
                                    dias_Solicitado=0,
                                    monto_Total=0

                                };
                                int resVaca = objCapaDatoVaca.registrar(obj2, out mensaje);
                                return resVaca;
                            }
                            else
                            {
                                mensaje = "No se puede enviar el correo";
                                return 0;
                            }

                        }
                        else
                        {
                            mensaje = "No se puede enviar el correo";
                            return 0;
                        }


                    }
                    else
                    {
                        mensaje = "No se puede enviar el correo";
                        return 0;
                    }

                    

                   

                    

                    
                }

            }
            else
            {
                return 0;
            }

            
        }


        public Boolean editar(persona obj, out string mensaje)
        {
            //Validaciones
            mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.nombre) || string.IsNullOrWhiteSpace(obj.nombre))
            {
                mensaje = "El nombre no puede estar vacio";

            }
            else if (string.IsNullOrEmpty(obj.apellido) || string.IsNullOrWhiteSpace(obj.nombre))
            {
                mensaje = "El apellido no puede estar vacio";

            }

            //creacion de clave automatica

            if (string.IsNullOrEmpty(mensaje))

            {
                {
                    

                    return objCapaDato.editar(obj, out mensaje);
                }

            }
            else
            {
                return false;
            }


           
        }

        public bool eliminar(int id, out string mensaje)
        {
            return objCapaDato.eliminar(id, out mensaje);   
        }

        public bool cambiarClave(int id,string nuevaClave, out string mensaje)
        {
            return objCapaDato.cambiarClave(id,nuevaClave, out mensaje);
        }

        public Boolean restablecerCorreo(int id, string correoEmpresarial, string contrasena,string usuario, out string mensaje)
        {
            mensaje = string.Empty;


            if (string.IsNullOrEmpty(mensaje))

            {
                {
                    string clave = CN_Recursos.generarClave();

                    string asunto = "Restablecer cuenta";
                    string mensaje_correo = "<body style=\"text-align: center; font-family: Arial, sans-serif;\">\r\n    <div style=\"margin-top: 50px;\">\r\n             <h2 style=\"margin-top: 20px;\">Contraseña nueva</h2>\r\n        <p>Estimado Usuario,</p>\r\n        <p>Se ha solicitado su usuario y contraseña nueva.</p>\r\n     <p>Su usuario es:</p>\r\n        <p style=\"font-size: 24px; font-weight: bold;\">!usuario!</p>\r\n   <p>Su nueva contraseña es:</p>\r\n        <p style=\"font-size: 24px; font-weight: bold;\">!clave!</p>\r\n        <p>Por favor, asegúrese de cambiar esta contraseña después de iniciar sesión.</p>\r\n        <p>Gracias,<br>El equipo de Sagicor</p>\r\n    </div>\r\n</body>";
                    mensaje_correo = mensaje_correo.Replace("!clave!", clave);
                    mensaje_correo = mensaje_correo.Replace("!usuario!",usuario);

                    bool respuesta = CN_Recursos.enviarCorreo(correoEmpresarial, asunto, mensaje_correo);

                    if (respuesta)
                    {
                        contrasena = CN_Recursos.ConvertirSha256(clave);
                        return objCapaDato.restablecerCorreo(id,correoEmpresarial,contrasena, out mensaje);
                    }
                    else
                    {
                        mensaje = "No se puede enviar el correo";
                        return false;
                    }




                }

            }
            else
            {
                return false;
            }


        }













        

    }
}
