using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace CapaNegocio
{
    public class CN_Planilla
    {


        private CD_Planilla objCapaDato = new CD_Planilla();


        public List<planilla> listar()
        {
            return objCapaDato.listar();
        }
        //Estoy treyendo el calulo para acá 

        public List<planilla> calcularRebajos(List<planilla> planillaSinCal)
        {
             CD_Permiso obj = new CD_Permiso();
             CD_Incapacidad objIncapacidad = new CD_Incapacidad();
             CD_Deducciones objDeducciones = new CD_Deducciones();
             CD_Persona objPersona = new CD_Persona();
           
             var ListPermiso = obj.listar();
             var mensaje = string.Empty;
            string fechaPago = string.Empty;


            foreach (var pla in planillaSinCal)
            {
                int mesActual = Convert.ToDateTime(pla.fecha_creacion).Month;
                int anoActual = Convert.ToDateTime(pla.fecha_creacion).Year;
                fechaPago= mesActual.ToString() + "/" + anoActual.ToString();
                var idPersona = pla.id_Persona;
                decimal salarioMensual = pla.salario_Total;
                decimal salarioXdia = pla.salario_Total / 21.65m;
                decimal salarioXhora = pla.salario_Total / (40 * 4.33m);
                decimal rebajoSalario = 0m;
                decimal salarioProporcional = 0m;
                decimal CCSS = 0m;

                var fechaIngresoPersona = objPersona.listar().FirstOrDefault(pe => pe.id_Persona ==pla.id_Persona);

                if (fechaIngresoPersona != null && DateTime.TryParse(fechaIngresoPersona.fecha_ingreso.ToString(), out DateTime fechaIngreso)) {
                    if (fechaIngreso.Month == mesActual && fechaIngreso.Year == anoActual)
                    {
                        // Calcular los días trabajados desde la fecha de ingreso hasta el final del mes
                        int diasTrabajados = 0;
                        DateTime fechaCorte = DateTime.Now; // Fecha actual o fin de mes

                        for (DateTime fecha = fechaIngreso; fecha <= fechaCorte; fecha = fecha.AddDays(1))
                        {
                            if (fecha.DayOfWeek != DayOfWeek.Saturday && fecha.DayOfWeek != DayOfWeek.Sunday)
                            {
                                diasTrabajados++;
                            }
                        }

                        salarioProporcional = salarioXdia * diasTrabajados;
                        CCSS = salarioProporcional * 0.1034m;
                        objCapaDato.actualizarIngresoNuevo(fechaIngresoPersona.id_Persona,salarioProporcional,CCSS,salarioProporcional,fechaPago,out mensaje);

                    }             
                }






                    var permisosDeLaPersona = ListPermiso.Where(pe =>
                {
                    DateTime fechaSolicitud;
                    bool esFechaValida = DateTime.TryParse(pe.fecha_Solicitud, out fechaSolicitud);

                    return pe.id_Persona == idPersona &&           // Permiso pertenece a la persona actual
                           pe.aprobado &&                          // Permiso está aprobado
                           esFechaValida &&                        // Fecha de solicitud válida
                           fechaSolicitud.Month == mesActual &&    // El mes coincide con el mes actual
                           fechaSolicitud.Year == anoActual &&       // El año coincide con el año actual
                           pe.concepto == "Sin goce de salario";
                }).ToList();

                foreach (var permiso in permisosDeLaPersona)
                {
                    //hace el calculo 
                    DateTime fechaHoraInicio = DateTime.Parse(permiso.hora_Inicio);
                    DateTime fechaHoraFinalizacion = DateTime.Parse(permiso.hora_Finalizacion);
                    TimeSpan diferencia = fechaHoraFinalizacion - fechaHoraInicio;
                    int dias = diferencia.Days;
                    int horas = diferencia.Hours;


                   decimal totalHorasLaborales = 0; 
                    decimal horasPorDiaLaboral = 8;

                    while (fechaHoraInicio < fechaHoraFinalizacion)
                    {
                        // Excluir fines de semana
                        if (fechaHoraInicio.DayOfWeek != DayOfWeek.Saturday && fechaHoraInicio.DayOfWeek != DayOfWeek.Sunday)
                        {
                            
                            DateTime horaInicioTrabajo = fechaHoraInicio.Date.AddHours(8);
                            DateTime horaFinTrabajo = fechaHoraInicio.Date.AddHours(17);

                         
                            DateTime finDia = (fechaHoraInicio.Date == fechaHoraFinalizacion.Date) ? fechaHoraFinalizacion : horaFinTrabajo;

                            
                            if (fechaHoraInicio < horaInicioTrabajo) fechaHoraInicio = horaInicioTrabajo;
                            if (finDia > horaFinTrabajo) finDia = horaFinTrabajo;

                            
                            totalHorasLaborales += (decimal)(finDia - fechaHoraInicio).TotalHours;
                        }

                        
                        fechaHoraInicio = fechaHoraInicio.Date.AddDays(1).AddHours(8);
                    }

                    // Calcular el total en días y horas sobrantes
                    decimal totalDiasLaborales = Math.Floor(totalHorasLaborales / horasPorDiaLaboral); // Días completos
                    decimal horasRestantes = totalHorasLaborales % horasPorDiaLaboral; // Horas restantes
                    rebajoSalario = rebajoSalario+ (totalDiasLaborales * salarioXdia) + (horas* salarioXhora);
                    
                }
                objCapaDato.actualizarPermiso(pla.id_Persona,rebajoSalario,pla.fecha_Pago, out mensaje);

            }

            //calcular incapacidad
            var listaIncapacidad = objIncapacidad.listar();

            foreach (var incapacidad in planillaSinCal)
            {

                int mesActual = Convert.ToDateTime(incapacidad.fecha_creacion).Month;
                int anoActual = Convert.ToDateTime(incapacidad.fecha_creacion).Year;
                DateTime fechaOriginal = DateTime.Parse(incapacidad.fecha_creacion);
                
                
                decimal incapacidaPagar = 0m;

                var incapacidadDeLaPersona = listaIncapacidad.Where(pe =>
                {
                    DateTime fechaSolicitud, fechaFinalizacion;
                    bool fechaInicioValida = DateTime.TryParse(pe.fecha_Solicitud, out fechaSolicitud);
                    bool fechaFinValida = DateTime.TryParse(pe.fecha_Finalizacion, out fechaFinalizacion);

                    return pe.id_Persona == incapacidad.id_Persona &&
                           fechaInicioValida && fechaFinValida &&
                           ((fechaSolicitud.Year < anoActual || (fechaSolicitud.Year == anoActual && fechaSolicitud.Month <= mesActual)) &&  // Fecha de inicio de incapacidad es antes o durante el mes actual
                           (fechaFinalizacion.Year > anoActual || (fechaFinalizacion.Year == anoActual && fechaFinalizacion.Month >= mesActual)));  // Fecha de fin de incapacidad es después o durante el mes actual
                }).ToList();


                foreach(var incapacidaPer in incapacidadDeLaPersona)
                { var res = true;      
                    int diasHabilidosPorMes = 22; // Días hábiles en el mes

                    if (incapacidaPer.aprobado == true) { 
                    
                            switch (incapacidaPer.tipo_Incapacidad)
                            {
                                case "Maternidad":
                          

                                    incapacidaPagar = incapacidaPagar+(incapacidad.salario_Total/ 2);
                                   res=objCapaDato.actualizarIncapacidad(incapacidad.id_Persona, incapacidaPagar,fechaPago,out mensaje);

                                    break;

                                case "Enfermedad común":
                                    decimal salarioDiario = incapacidad.salario_Total / diasHabilidosPorMes; 
                                    decimal total = 0;
                                    decimal totalPagarIncapacidad = 0;
                            
                                    int diasHabilidos = 0;
                                    int diasNoHabilidos = 0;  

                            
                                    for (DateTime fecha = DateTime.Parse(incapacidaPer.fecha_Inicio); fecha <= DateTime.Parse(incapacidaPer.fecha_Finalizacion); fecha = fecha.AddDays(1))
                                    {
                               
                                        if (fecha.DayOfWeek != DayOfWeek.Saturday && fecha.DayOfWeek != DayOfWeek.Sunday)
                                        {
                                            diasHabilidos++;
                                        }
                                        else
                                        {
                                            diasNoHabilidos++;  
                                        }
                                    }

                            
                                    if (diasHabilidos <= 3)
                                    {
                                        totalPagarIncapacidad = totalPagarIncapacidad +(salarioDiario * diasHabilidos * 0.50m);  
                                    }
                                    else
                                    {
                                        totalPagarIncapacidad = totalPagarIncapacidad + (salarioDiario * 3  * 0.50m);  
                                    }

                                    total = diasHabilidos * salarioDiario;




                                    res = objCapaDato.actualizarIncapacidadEnfermedad(incapacidad.id_Persona, total, fechaPago,totalPagarIncapacidad, out mensaje);
                                    break;

                                case "Riesgo de trabajo":

                                     salarioDiario = incapacidad.salario_Total / diasHabilidosPorMes;
                                     total = 0;
                                     diasHabilidos = 0;
                           

                                    // Contar los días hábiles y calcular el total a pagar
                                    for (DateTime fecha = DateTime.Parse(incapacidaPer.fecha_Inicio); fecha <= DateTime.Parse(incapacidaPer.fecha_Finalizacion); fecha = fecha.AddDays(1))
                                    {
                                        // Contar solo días de lunes a viernes
                                        if (fecha.DayOfWeek != DayOfWeek.Saturday && fecha.DayOfWeek != DayOfWeek.Sunday)
                                        {
                                            diasHabilidos++;
                                        }
                                    }

                            
                                    totalPagarIncapacidad = (salarioDiario * diasHabilidos);
                           
                                    res = objCapaDato.actualizarIncapacidadEnfermedad(incapacidad.id_Persona, total,fechaPago,totalPagarIncapacidad, out mensaje);
                                    break;


                                case "Licencias de fase terminal":
                                    diasHabilidos = 0;
                                    total = 0;
                                    salarioDiario = incapacidad.salario_Total / diasHabilidosPorMes;
                                    // Contar los días hábiles y calcular el total a pagar
                                    for (DateTime fecha = DateTime.Parse(incapacidaPer.fecha_Inicio); fecha <= DateTime.Parse(incapacidaPer.fecha_Finalizacion); fecha = fecha.AddDays(1))
                                    {
                                        // Contar solo días de lunes a viernes
                                        if (fecha.DayOfWeek != DayOfWeek.Saturday && fecha.DayOfWeek != DayOfWeek.Sunday)
                                        {
                                            diasHabilidos++;
                                        }
                                        total =diasHabilidos * salarioDiario;
                                        totalPagarIncapacidad = diasHabilidos * salarioDiario;



                                        res = objCapaDato.actualizarIncapacidadEnfermedad(incapacidad.id_Persona, total, fechaPago, totalPagarIncapacidad, out mensaje);
                                    }

                                    break;


                                case "Licencia para cuidado de persona menor gravemente enferma":
                                    diasHabilidos = 0;
                                    total = 0;
                                    salarioDiario = incapacidad.salario_Total / diasHabilidosPorMes;
                                    // Contar los días hábiles y calcular el total a pagar
                                    for (DateTime fecha = DateTime.Parse(incapacidaPer.fecha_Inicio); fecha <= DateTime.Parse(incapacidaPer.fecha_Finalizacion); fecha = fecha.AddDays(1))
                                    {
                                        // Contar solo días de lunes a viernes
                                        if (fecha.DayOfWeek != DayOfWeek.Saturday && fecha.DayOfWeek != DayOfWeek.Sunday)
                                        {
                                            diasHabilidos++;
                                        }
                                        total = diasHabilidos * salarioDiario;
                                        totalPagarIncapacidad = diasHabilidos * salarioDiario;



                                        res = objCapaDato.actualizarIncapacidadEnfermedad(incapacidad.id_Persona, total, fechaPago, totalPagarIncapacidad, out mensaje);
                                    }

                                    break;

                            }
                    
                    
                    }






                 
           

                }


            }
            //Rebajos de pensiones y embargos
            var listaDeducciones = objDeducciones.listar();
          
            var dedu = new deducciones();
            
            foreach (var persona in planillaSinCal)
            {
                // Verifica que la fecha de creación de la persona sea válida
                if (!DateTime.TryParse(persona.fecha_creacion, out DateTime fechaOriginal))
                {
                    // Manejar caso donde la fecha no es válida
                    continue; // O lanzar una excepción
                }


                var deduccionesDeLaPersona = listaDeducciones.Where(pe =>
                {

                    if (!DateTime.TryParse(pe.fecha_Inicio, out DateTime fechaInicio) ||
                        !DateTime.TryParse(pe.fecha_Finalizacion, out DateTime fechaFinalizacion))
                    {
                        return false;
                    }


                    return pe.id_Persona == persona.id_Persona &&
                           fechaInicio <= fechaOriginal &&
                           fechaFinalizacion >= fechaOriginal;
                }).ToList();




                if (deduccionesDeLaPersona.Count > 0)
                {
                    var totalDeducciones = 0m;
                    foreach (var deducciones in deduccionesDeLaPersona)
                    {
                        if (deducciones.activo) { 
                            totalDeducciones = totalDeducciones + deducciones.monto_Cuota;
                            objCapaDato.actualizarDeducciones(deducciones.id_Persona, deducciones.id_Deduccion,out mensaje);
                        }
                    
                    }
                    
                    objCapaDato.actualizarPension(persona.id_Persona, dedu.monto_Cuota=totalDeducciones, fechaPago, out mensaje);
                }



               

            }

            return objCapaDato.listar();
        }


        public int registrar(planilla obj, out string mensaje)
        {
            //Validaciones
            mensaje = string.Empty;

            //creacion de clave automatica
            return objCapaDato.registrar(obj, out mensaje);


        }

        public Boolean editar(planilla obj, out string mensaje)
        {


            return objCapaDato.editar(obj, out mensaje);



        }

        public bool eliminar(int id, out string mensaje)
        {
            return objCapaDato.eliminar(id, out mensaje);
        }

        public bool calculoPlanilla()
        {
            return objCapaDato.calculoPlanilla();
        }


        
    


    }
}
