
using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CapaNegocio
{
    public class CN_Notificacion
    {


        private CD_Notificacion objCapaDato = new CD_Notificacion();
        private CD_Persona objCapaDatoPersona = new CD_Persona();

        public List<notificaciones> listar()
        {
            return objCapaDato.listar();
        }

        public List<listarSolicitudes> listarSolicitudes(int idDepartamento)
        {
            return objCapaDato.listarSolicitudes(idDepartamento);
        }

        public List<listarSolicitudes> listarSolicitudesRRHH()
        {
            return objCapaDato.listarSolicitudesRRHH();
        }

        public List<listarSolicitudes> listarSolicitudesEmpleado()
        {
            return objCapaDato.listarSolicitudesEmpleado();
        }

        public List<listarSolicitudes> listarSolicitudesGerente()
        {
            return objCapaDato.listarSolicitudesGerente();
        }

        public Boolean actualizarNotificacion(int id_Noti, int estado, int aprobacionRRHH)
        {




            return objCapaDato.actualizarEstado(id_Noti, estado, aprobacionRRHH);
        }


        public int registrar(notificaciones obj, out string mensaje)
        {
            //Validaciones
            mensaje = string.Empty;

            //creacion de clave automatica
            return objCapaDato.registrar(obj, out mensaje);


        }



        public Boolean editar(notificaciones obj, out string mensaje)
        {


            return objCapaDato.editar(obj, out mensaje);



        }

        public bool eliminar(int id, out string mensaje)
        {
            return objCapaDato.eliminar(id, out mensaje);
        }

        public bool eliminarSolicitud(int id, out string mensaje)
        {
            int Persona = 0;

            List<notificaciones> listaNoti = (List<notificaciones>)objCapaDato.listar();

            var notificacionAEliminar = listaNoti.FirstOrDefault(n => n.id_Noti == id);
            if(notificacionAEliminar!= null)
            {
                Persona = notificacionAEliminar.id_Persona;
                List<persona> listCorreo = (List <persona>)objCapaDatoPersona.listar();
                var idPersona = listCorreo.FirstOrDefault(a => a.id_Persona == Persona);
                string asunto = "Cancelación de "+notificacionAEliminar.tipo_Notificacion;
                string mensaje_correo = "<style>\r\n        body {\r\n            font-family: Arial, sans-serif;\r\n            background-color: #f4f4f9;\r\n            color: #333;\r\n            margin: 0;\r\n            padding: 20px;\r\n        }\r\n        .container {\r\n            max-width: 600px;\r\n            margin: 0 auto;\r\n            background-color: #fff;\r\n            padding: 20px;\r\n            border-radius: 8px;\r\n            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);\r\n        }\r\n        h1 {\r\n            color: #d9534f;\r\n        }\r\n        .message {\r\n            margin: 20px 0;\r\n            line-height: 1.6;\r\n        }\r\n        .footer {\r\n            margin-top: 20px;\r\n            text-align: center;\r\n            color: #777;\r\n        }\r\n    </style>\r\n</head>\r\n<body>\r\n    <div class=\"container\">\r\n        <h1>Solicitud Cancelada</h1>\r\n        <div class=\"message\">\r\n            <p>Estimado/a [Nombre del Empleado],</p>\r\n            <p>Lamentamos informarle que su [vacaciones] ha sido cancelada.</p>\r\n            <p>Para más información, por favor póngase en contacto con su líder.</p>\r\n        </div>\r\n        <div class=\"footer\">\r\n            <p>Atentamente,</p>\r\n            <p>Sagicor</p>\r\n        </div>\r\n    </div>\r\n</body>";
                mensaje_correo = mensaje_correo.Replace("[Nombre del Empleado]", idPersona.nombre + " " + idPersona.apellido);
                mensaje_correo = mensaje_correo.Replace("[vacaciones]", notificacionAEliminar.tipo_Notificacion);
                bool respuesta = CN_Recursos.enviarCorreo(idPersona.correo_Empresarial, asunto, mensaje_correo);
            }


            return objCapaDato.eliminar(id, out mensaje);
        }




        public int solicitudVacaciones(notificaciones obj,decimal saldo_vacaciones , persona Persona , out string mensaje)
        {
            try
            {


                //Validaciones
                mensaje = string.Empty;
                //Validaciones
                if (obj.dias_Solicitado <= saldo_vacaciones )
                {
                    //Enviar al correo la solicitud creada de vacaciones

                    string asunto = "Solicitud de vacaciones";
                    string mensaje_correo = "<body>\r\n <style>\r\n        body {\r\n            font-family: Arial, sans-serif;\r\n            line-height: 1.6;\r\n        }\r\n        .container {\r\n            max-width: 600px;\r\n            margin: 0 auto;\r\n            padding: 20px;\r\n            border: 1px solid #ddd;\r\n            border-radius: 5px;\r\n            background-color: #f9f9f9;\r\n        }\r\n        .header {\r\n            background-color: #007BFF;\r\n            color: #ffffff;\r\n            padding: 10px;\r\n            text-align: center;\r\n            border-radius: 5px 5px 0 0;\r\n        }\r\n        .content {\r\n            padding: 20px;\r\n        }\r\n        .content p {\r\n            margin: 10px 0;\r\n        }\r\n        .footer {\r\n            text-align: center;\r\n            margin-top: 20px;\r\n            font-size: 0.9em;\r\n            color: #555;\r\n        }\r\n    </style>\r\n    <div class=\"container\">\r\n        <div class=\"header\">\r\n            <h1>Solicitud de Vacaciones</h1>\r\n        </div>\r\n        <div class=\"content\">\r\n            <p><strong>Nombre del empleado:</strong> !usuario!</p>\r\n            <p><strong>Cantidad de días solicitados:</strong>!diasSolicitado!</p>\r\n            <p><strong>Días solicitados:</strong>!fechaSolicitadas!</p>\r\n        </div>\r\n        <div class=\"footer\">\r\n            <p>Este es un mensaje automático. Por favor, no responda a este correo.</p>\r\n        </div>\r\n    </div>\r\n</body>\r\n\r\n\r\n\r\n\r\n";
                    mensaje_correo = mensaje_correo.Replace("!usuario!", Persona.nombre+" " + Persona.apellido);
                    mensaje_correo = mensaje_correo.Replace("!diasSolicitado!", obj.dias_Solicitado.ToString());
                    mensaje_correo = mensaje_correo.Replace("!fechaSolicitadas!", obj.fechasSolicitadas.TrimStart(','));

                    bool respuesta = CN_Recursos.enviarCorreo(Persona.correo_Empresarial, asunto, mensaje_correo);

                   string lideres = objCapaDato.BuscarLider(Persona.id_Departamento, out mensaje);

                    //correo para lider
                
                    string mensaje_correoLider = " <body>\r\n    <style>\r\n        body {\r\n            font-family: Arial, sans-serif;\r\n            background-color: #f4f4f4;\r\n            margin: 0;\r\n            padding: 0;\r\n        }\r\n        .container {\r\n            width: 80%;\r\n            margin: 0 auto;\r\n            background-color: #ffffff;\r\n            padding: 20px;\r\n            border-radius: 10px;\r\n            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);\r\n        }\r\n        .header {\r\n            background-color: #4CAF50;\r\n            color: white;\r\n            padding: 10px 0;\r\n            text-align: center;\r\n            border-radius: 10px 10px 0 0;\r\n        }\r\n        .content {\r\n            margin: 20px 0;\r\n        }\r\n        .footer {\r\n            margin-top: 20px;\r\n            text-align: center;\r\n            color: #888;\r\n            font-size: 12px;\r\n        }\r\n    </style>\r\n    <div class=\"container\">\r\n        <div class=\"header\">\r\n            <h1>Solicitud de Vacaciones</h1>\r\n        </div>\r\n        <div class=\"content\">\r\n            <p>Estimado/a Usuario,</p>\r\n            <p>Le informamos que el empleado <strong>!usuario!</strong> ha solicitado vacaciones las siguientes fechas <strong>!Fecha de Inicio!</strong>.</p>\r\n            <p>A continuación, se detallan los datos de la solicitud:</p>\r\n            <ul>\r\n                <li><strong>Nombre del Empleado:</strong>!usuario!</li>\r\n                <li><strong>Departamento:</strong> !Departamento!</li>\r\n                <li><strong>Fechas solicitas:</strong> !Fecha de Inicio!</li>\r\n                \r\n                       </ul>\r\n            <p>Por favor, revise y gestione esta solicitud a la mayor brevedad posible.</p>\r\n            <p>Gracias por su atención.</p>\r\n        </div>\r\n        <div class=\"footer\">\r\n            <p>Este es un mensaje generado automáticamente. Por favor, no responda a este correo.</p>\r\n        </div>\r\n    </div>\r\n</body>";
                    mensaje_correoLider = mensaje_correoLider.Replace("!usuario!", Persona.nombre+" "+Persona.apellido);
                    mensaje_correoLider = mensaje_correoLider.Replace("!Departamento!", Persona.id_Departamento.ToString());
                    mensaje_correoLider = mensaje_correoLider.Replace("!Fecha de Inicio!", obj.fechasSolicitadas.TrimStart(','));

                    bool respuestaLider = CN_Recursos.enviarCorreo(lideres,asunto,mensaje_correoLider);

                    return objCapaDato.registrar(obj, out mensaje);

                }else
                {
                    mensaje="No tienes suficientes días aculados, intente con una catidad menor";
                    return 0;

                }








            }
            catch (Exception ex)
            {

                mensaje = ex.Message;
                return 0;
            }




            



            //creacion de clave automatica
            


        }
        //SolicitarPermiso

        public int solicitudPermiso(notificaciones obj, persona Persona,permiso permiso, out string mensaje)
        {
            //Validaciones
            mensaje = string.Empty;

 
            //Validaciones
            if (Persona!=null)
            {
                //Enviar al correo la solicitud creada de permiso

                string asunto = "Solicitud de permiso";
                string mensaje_correo = "<body>\r\n <style>\r\n        body {\r\n            font-family: Arial, sans-serif;\r\n            line-height: 1.6;\r\n        }\r\n        .container {\r\n            max-width: 600px;\r\n            margin: 0 auto;\r\n            padding: 20px;\r\n            border: 1px solid #ddd;\r\n            border-radius: 5px;\r\n            background-color: #f9f9f9;\r\n        }\r\n        .header {\r\n            background-color: #007BFF;\r\n            color: #ffffff;\r\n            padding: 10px;\r\n            text-align: center;\r\n            border-radius: 5px 5px 0 0;\r\n        }\r\n        .content {\r\n            padding: 20px;\r\n        }\r\n        .content p {\r\n            margin: 10px 0;\r\n        }\r\n        .footer {\r\n            text-align: center;\r\n            margin-top: 20px;\r\n            font-size: 0.9em;\r\n            color: #555;\r\n        }\r\n    </style>\r\n    <div class=\"container\">\r\n        <div class=\"header\">\r\n            <h1>Solicitud de Permiso</h1>\r\n        </div>\r\n        <div class=\"content\">\r\n            <p><strong>Nombre del empleado:</strong> !usuario!</p>\r\n            <p><strong>Fecha de solicitud:</strong>!Fecha!</p>\r\n            <p><strong>Hora de inicio:</strong>!HoraInicio!</p>\r\n     <p><strong>Hora de finalización </strong>!horaFinalizacion!</p>\r\n   </div>\r\n        <div class=\"footer\">\r\n            <p>Este es un mensaje automático. Por favor, no responda a este correo.</p>\r\n        </div>\r\n    </div>\r\n</body>\r\n\r\n\r\n\r\n\r\n";
                mensaje_correo = mensaje_correo.Replace("!usuario!", Persona.nombre + " " + Persona.apellido);
                mensaje_correo = mensaje_correo.Replace("!Fecha!", permiso.fecha_Solicitud.ToString());
                mensaje_correo = mensaje_correo.Replace("!HoraInicio!", permiso.hora_Inicio.ToString());
                mensaje_correo = mensaje_correo.Replace("!horaFinalizacion!", permiso.hora_Finalizacion.ToString());

                bool respuesta = CN_Recursos.enviarCorreo(Persona.correo_Empresarial, asunto, mensaje_correo);

                string lideres = objCapaDato.BuscarLider(Persona.id_Departamento, out mensaje);

                //correo para lider

                string mensaje_correoLider = " <body>\r\n    <style>\r\n        body {\r\n            font-family: Arial, sans-serif;\r\n            background-color: #f4f4f4;\r\n            margin: 0;\r\n            padding: 0;\r\n        }\r\n        .container {\r\n            width: 80%;\r\n            margin: 0 auto;\r\n            background-color: #ffffff;\r\n            padding: 20px;\r\n            border-radius: 10px;\r\n            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);\r\n        }\r\n        .header {\r\n            background-color: #4CAF50;\r\n            color: white;\r\n            padding: 10px 0;\r\n            text-align: center;\r\n            border-radius: 10px 10px 0 0;\r\n        }\r\n        .content {\r\n            margin: 20px 0;\r\n        }\r\n        .footer {\r\n            margin-top: 20px;\r\n            text-align: center;\r\n            color: #888;\r\n            font-size: 12px;\r\n        }\r\n    </style>\r\n    <div class=\"container\">\r\n        <div class=\"header\">\r\n            <h1>Solicitud de Permiso</h1>\r\n        </div>\r\n        <div class=\"content\">\r\n            <p>Estimado/a Usuario,</p>\r\n            <p>Le informamos que el empleado <strong>!usuario!</strong> ha solicitado un permiso para la siguiente fecha <strong>!Fecha de Inicio!</strong>.</p>\r\n            <p>A continuación, se detallan los datos de la solicitud:</p>\r\n            <ul>\r\n                <li><strong>Nombre del Empleado:</strong>!usuario!</li>\r\n                <li><strong>Hora de inicio:</strong> !horaInicio!</li>\r\n                <li><strong>Hora de finalización:</strong> !horaFinalizacion!</li>\r\n                \r\n                       </ul>\r\n            <p>Por favor, revise y gestione esta solicitud a la mayor brevedad posible.</p>\r\n            <p>Gracias por su atención.</p>\r\n        </div>\r\n        <div class=\"footer\">\r\n            <p>Este es un mensaje generado automáticamente. Por favor, no responda a este correo.</p>\r\n        </div>\r\n    </div>\r\n</body>";
                mensaje_correoLider = mensaje_correoLider.Replace("!usuario!", Persona.nombre + " " + Persona.apellido);
                mensaje_correoLider = mensaje_correoLider.Replace("!Fecha de Inicio!", permiso.fecha_Solicitud);
                mensaje_correoLider = mensaje_correoLider.Replace("!horaInicio!", permiso.hora_Inicio.ToString());
                mensaje_correoLider = mensaje_correoLider.Replace("!horaFinalizacion!", permiso.hora_Finalizacion.ToString());

                bool respuestaLider = CN_Recursos.enviarCorreo(lideres, asunto, mensaje_correoLider);

                return objCapaDato.registrar(obj, out mensaje);

            }
            else
            {
                mensaje = "Error al realizar la solicitud";
                return 0;

            }

        }
        //Envio de correo de la incapacidad
        public int EnviarIncapacidad(persona Persona,incapacidad Incapacidad,HttpPostedFileBase documento){
             var mensaje = string.Empty;
            //bUSCA EL LIDER 
            string lideres = objCapaDato.BuscarLider(Persona.id_Departamento, out mensaje);

            string asunto="Solicitud de incapacidad";
            string mensaje_correo = " <body style=\"font-family: Arial, sans-serif; color: #333333; line-height: 1.6;\">\r\n\r\n                <div style=\"max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #dddddd; border-radius: 5px;                                 background-color:               #f9f9f9;\">\r\n                <h2 style=\"text-align: center; color: #4CAF50;\">Notificación de Solicitud de Incapacidad</h2>\r\n\r\n                <p>Estimados líderes,</p>\r\n\r\n                <p>Le informamos que el usuario <strong>[Nombre del Usuario]</strong> ha presentado una solicitud de incapacidad a partir del <strong>[Fecha de Inicio]</strong> hasta el <strong>[Fecha de Finalización]</strong>. A continuación, se detallan los                   motivos y la documentación relacionada con su solicitud:</p>\r\n\r\n                            <li><strong>Motivo de la Incapacidad:</strong> [Motivo de la Incapacidad]</li>\r\n                <li><strong>Documentación Adjunta:</strong> [Nombre del Documento] (adjunto)</li>\r\n                </ul>\r\n\r\n                <p>Por favor, revise la solicitud y tome las medidas necesarias. Si tiene alguna pregunta o necesita más información, no dude ponerse en contacto con el Departamento de Recursos Humanos.</p>\r\n\r\n                 <p>Gracias,</p>\r\n                                 </div>\r\n\r\n                </body>";
                mensaje_correo=mensaje_correo.Replace("[Nombre del Usuario]",Persona.nombre+" "+Persona.apellido);
                mensaje_correo=mensaje_correo.Replace("[Fecha de Inicio]",Incapacidad.fecha_Inicio.ToString());
                mensaje_correo=mensaje_correo.Replace("[Fecha de Finalización]",Incapacidad.fecha_Finalizacion.ToString());
                mensaje_correo=mensaje_correo.Replace("[Motivo de la Incapacidad]",Incapacidad.tipo_Incapacidad.ToString() );
                mensaje_correo=mensaje_correo.Replace("[Nombre del Documento] (adjunto)",documento.FileName);
                //queda piendiente revisar lo del documento 

            bool resp= CN_Recursos.enviarCorreocconAdjunto(lideres,asunto,mensaje_correo,documento);


            if (resp) {


                //Enviar al correo la solicitud creada de permiso

                mensaje_correo = mensaje_correo.Replace("líderes","");

                bool respuesta = CN_Recursos.enviarCorreo(Persona.correo_Empresarial, asunto, mensaje_correo);




                return 1;
            }
            else
            {
                return 0;
            }
           
        }

        public Boolean solicitudVacacionesRRHH(listarSolicitudes obj, persona Persona, out string mensaje)
        {
            //Validaciones
            mensaje = string.Empty;
            //Validaciones
          
         
                

                string asunto = "Aprobación de "+obj.tipo_Notificacion;
                string mensaje_correo = "<!DOCTYPE html>\r\n<html lang=\"es\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <title>Notificación de Nueva Solicitud</title>\r\n    <style>\r\n        body {\r\n            font-family: Arial, sans-serif;\r\n            background-color: #f4f4f4;\r\n            margin: 0;\r\n            padding: 0;\r\n        }\r\n        .container {\r\n            background-color: #ffffff;\r\n            margin: 20px auto;\r\n            padding: 20px;\r\n            border-radius: 8px;\r\n            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);\r\n            max-width: 600px;\r\n        }\r\n        .header {\r\n            background-color: #007bff;\r\n            color: white;\r\n            padding: 10px;\r\n            text-align: center;\r\n            border-radius: 8px 8px 0 0;\r\n        }\r\n        .content {\r\n            margin: 20px 0;\r\n        }\r\n        .content h2 {\r\n            color: #007bff;\r\n        }\r\n        .content p {\r\n            margin: 5px 0;\r\n            line-height: 1.6;\r\n        }\r\n        .footer {\r\n            text-align: center;\r\n            color: #666666;\r\n            margin-top: 20px;\r\n        }\r\n        .footer a {\r\n            color: #007bff;\r\n            text-decoration: none;\r\n        }\r\n        .btn {\r\n            background-color: #007bff;\r\n            color: white;\r\n            padding: 10px 20px;\r\n            text-decoration: none;\r\n            border-radius: 5px;\r\n            display: inline-block;\r\n        }\r\n        .btn:hover {\r\n            background-color: #0056b3;\r\n        }\r\n        .table {\r\n            width: 100%;\r\n            border-collapse: collapse;\r\n            margin-top: 15px;\r\n        }\r\n        .table th, .table td {\r\n            border: 1px solid #dddddd;\r\n            text-align: left;\r\n            padding: 8px;\r\n        }\r\n        .table th {\r\n            background-color: #f2f2f2;\r\n        }\r\n    </style>\r\n</head>\r\n<body>\r\n    <div class=\"container\">\r\n        <div class=\"header\">\r\n            <h1>Nueva Solicitud de Aprobación</h1>\r\n        </div>\r\n\r\n        <div class=\"content\">\r\n            <h2>¡Hola,Departamento de Recursos Humanos!</h2>\r\n            <p>Tienes una nueva solicitud que requiere aprobación. A continuación se muestran los detalles de la solicitud:</p>\r\n\r\n            <table class=\"table\">\r\n                <tr>\r\n                    <th>Nombre del Aprobador</th>\r\n                    <td>[NombredelAprobador]</td>\r\n                </tr>\r\n                <tr>\r\n                    <th>Tipo de Solicitud</th>\r\n                    <td>[TipodeSolicitud]</td>\r\n                </tr>\r\n                <tr>\r\n                    <th>Fechas Solicitadas</th>\r\n                    <td>[FechasSolicitadas]</td>\r\n                </tr>\r\n                <tr>\r\n                    <th>Días Solicitados</th>\r\n                    <td>[DíasSolicitados]</td>\r\n                </tr>\r\n                <tr>\r\n                    <th>Horas Solicitadas</th>\r\n                    <td>[HorasSolicitadas]</td>\r\n                </tr>\r\n                   <tr>\r\n                    <th>Id del empleado</th>\r\n                    <td>[id_empleado]</td>\r\n                </tr>\r\n            </table>\r\n\r\n            \r\n        </div>\r\n\r\n        <div class=\"footer\">\r\n            <p>Este es un correo automático, por favor no responder.</p>\r\n           \r\n        </div>\r\n    </div>\r\n</body>\r\n</html>\r\n";
                mensaje_correo = mensaje_correo.Replace("[NombredelAprobador]", Persona.nombre + " " + Persona.apellido);
                mensaje_correo = mensaje_correo.Replace("[TipodeSolicitud]", obj.tipo_Notificacion);
                mensaje_correo = mensaje_correo.Replace("[FechasSolicitadas]", obj.fechasSolicitadas.TrimStart(','));
                mensaje_correo = mensaje_correo.Replace("[DíasSolicitados]", obj.dias_Solicitado.ToString());
                mensaje_correo = mensaje_correo.Replace("[HorasSolicitadas]", obj.horas_solicitadas.ToString());
                mensaje_correo = mensaje_correo.Replace("[id_empleado]", obj.id_Persona.ToString());


                bool respuesta = CN_Recursos.enviarCorreo("lily_zuniga@sagicorhr.es", asunto, mensaje_correo);
                                      
          
                return respuesta;

               
            
        }

        public Boolean solicitudAprobadaRRHH(listarSolicitudes obj, persona Persona, out string mensaje)
        {
            //Validaciones
            mensaje = string.Empty;
            //Validaciones


            List<persona> lista = new List<persona>();
            lista = objCapaDatoPersona.listar();

            var personaEncontrada = lista.FirstOrDefault(p => p.id_Persona == obj.id_Persona);

            string asunto = "Aprobación de " + obj.tipo_Notificacion;
            string mensaje_correo = "<!DOCTYPE html>\r\n<html lang=\"es\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <title>Aprobación de Solicitud</title>\r\n    <style>\r\n        body {\r\n            font-family: Arial, sans-serif;\r\n            background-color: #f4f4f4;\r\n            margin: 0;\r\n            padding: 0;\r\n        }\r\n        .container {\r\n            background-color: #ffffff;\r\n            width: 100%;\r\n            max-width: 600px;\r\n            margin: 20px auto;\r\n            border-radius: 10px;\r\n            box-shadow: 0 2px 5px rgba(0,0,0,0.1);\r\n            padding: 20px;\r\n        }\r\n        h2 {\r\n            color: #4CAF50;\r\n            text-align: center;\r\n        }\r\n        .details {\r\n            margin: 20px 0;\r\n            font-size: 16px;\r\n        }\r\n        .details p {\r\n            margin: 10px 0;\r\n        }\r\n        .button {\r\n            display: block;\r\n            width: 200px;\r\n            margin: 20px auto;\r\n            text-align: center;\r\n        }\r\n        .button a {\r\n            background-color: #4CAF50;\r\n            color: white;\r\n            padding: 10px 15px;\r\n            text-decoration: none;\r\n            border-radius: 5px;\r\n        }\r\n        .footer {\r\n            text-align: center;\r\n            font-size: 12px;\r\n            color: #888;\r\n            margin-top: 20px;\r\n        }\r\n    </style>\r\n</head>\r\n<body>\r\n\r\n    <div class=\"container\">\r\n        <h2>Solicitud Aprobada</h2>\r\n\r\n        <div class=\"details\">\r\n            <p><strong>Estimado empleado,</strong></p>\r\n            <p>Nos complace informarte que tu solicitud ha sido <strong>aprobada</strong> por el aprobador <strong>Recursos Humanos</strong>.</p>\r\n\r\n            <p>Detalles de la solicitud:</p>\r\n            <ul>\r\n                <li><strong>Tipo de solicitud:</strong> [Tipo de Solicitud]</li>\r\n                <li><strong>Fechas solicitadas:</strong> [Fechas Solicitadas]</li>\r\n                <li><strong>Días solicitados:</strong> [Días Solicitados]</li>\r\n                <li><strong>Horas solicitadas:</strong> [Horas Solicitadas]</li>\r\n            </ul>\r\n\r\n           \r\n        <div class=\"footer\">\r\n            <p>Este es un mensaje generado automáticamente. No responda a este correo.</p>\r\n        </div>\r\n    </div>\r\n\r\n</body>\r\n</html>\r\n";
            
            mensaje_correo = mensaje_correo.Replace("[Tipo de Solicitud]", obj.tipo_Notificacion);
            mensaje_correo = mensaje_correo.Replace("[Fechas Solicitadas]", obj.fechasSolicitadas.TrimStart(','));
            mensaje_correo = mensaje_correo.Replace("[Días Solicitados]", obj.dias_Solicitado.ToString());
            mensaje_correo = mensaje_correo.Replace("[Horas Solicitadas]", obj.horas_solicitadas.ToString());

            bool aprobacion = objCapaDato.aprobarPersimo(personaEncontrada.id_Persona);
            bool respuesta = false;
            if (aprobacion)
            {
                respuesta = CN_Recursos.enviarCorreo(personaEncontrada.correo_Empresarial, asunto, mensaje_correo);
            }
            else {
                respuesta = false;
            }
        


            return respuesta;



        }





    }

}
