using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Liquidacion
    {


        private CD_Liquidacion objCapaDato = new CD_Liquidacion();
        private CD_Aguinaldo objCapaDatoAgui = new CD_Aguinaldo();

        public List<liquidacion> listar()
        {
            return objCapaDato.listar();
        }


        public int registrar(liquidacion obj, out string mensaje)
        {
            //Validaciones
            mensaje = string.Empty;

            //creacion de clave automatica
            return objCapaDato.registrar(obj, out mensaje);


        }

        public Boolean editar(liquidacion obj, out string mensaje)
        {


            return objCapaDato.editar(obj, out mensaje);



        }

        public bool eliminar(int id, out string mensaje)
        {
            return objCapaDato.eliminar(id, out mensaje);
        }



        public List<liquidacion> calcularLiquidacion(int id_Persona,string tipo_Liquidacion,string tipoPreaviso, DateTime fechaSalida)
        {

            try
            {
                decimal salarioMesual = 0m;
                int anosTrabajados = 0;
                int mesesTrabajados = 0;
                decimal diasVacaciones = 0m;
                string preaviso = tipoPreaviso;
                decimal cesantia = 0m;
                decimal preavisoDinero = 0m;
                decimal aguinaldo = 0m;
                decimal totalPagar = 0m;
                
                decimal vacaciones = 0m;


                string mensaje = string.Empty;
                List<liquidacion> calculado = new List<liquidacion>();

                List<PersonaLiquidacion> listaPersonas = objCapaDato.buscarPersona(id_Persona, out mensaje);

                if (listaPersonas != null || listaPersonas.Count > 0)
                {
                    foreach (var persona in listaPersonas)
                    {
                        salarioMesual = persona.salario_Bruto;
                        DateTime fechaIngreso = Convert.ToDateTime(persona.fecha_ingreso);
                        DateTime fechaActual = fechaSalida;

                        // Calcular años completos trabajados
                        anosTrabajados = fechaActual.Year - fechaIngreso.Year;

                        // Ajustar si el mes actual es anterior al mes de ingreso en el mismo año
                        if (fechaActual.Month < fechaIngreso.Month ||
                            (fechaActual.Month == fechaIngreso.Month && fechaActual.Day < fechaIngreso.Day))
                        {
                            anosTrabajados--;
                        }

                        // Calcular los meses trabajados después de los años completos
                        int mesesAdicionales = (fechaActual.Year * 12 + fechaActual.Month) - (fechaIngreso.Year * 12 + fechaIngreso.Month);

                        // Ajustar los meses adicionales a fracción de año
                        mesesTrabajados = mesesAdicionales % 12;  // 

                        diasVacaciones = persona.saldo_Vacaciones;

                        if (anosTrabajados >= 1)
                        {
                            // Calcular los años calculables (máximo 8)
                            int anosCalculables = Math.Min(anosTrabajados, 8);



                            // Calcular cesantía
                            cesantia = (salarioMesual / 12) * (anosCalculables + (mesesTrabajados));
                        }


                        List<aguinaldoMeses> olista = new List<aguinaldoMeses>();
                        olista = new CN_Aguinaldo().listarMeses();

                        List<aguinaldoMeses> listaFiltrada = olista.Where(m => m.id_Persona == persona.id_Persona).ToList();

                        foreach (var totalAguinaldo in listaFiltrada)
                        {
                            aguinaldo = totalAguinaldo.TotalAnio / 12;
                        }

                        vacaciones = (salarioMesual / 30) * persona.saldo_Vacaciones;


                        if (tipoPreaviso.ToLower() == "si")
                        {
                            if (anosTrabajados >= 1)
                            {
                                preavisoDinero = persona.salario_Bruto; // 1 mes de salario
                            }
                            else if (mesesTrabajados >= 6)
                            {
                                preavisoDinero = (persona.salario_Bruto / 2); // 15 días
                            }
                            else if (mesesTrabajados >= 3)
                            {
                                preavisoDinero = (persona.salario_Bruto / 4); // 1 semana
                            }
                        }
                        var salarioPendiente = 0m;


                        if (tipo_Liquidacion.ToLower() == "renuncia" || tipo_Liquidacion.ToLower() == "despido sin responsabilidad") {
                            var salarioDiario = salarioMesual / 30;
                            int diasTrabajados = 0;
                            for (int day = 1; day <= fechaSalida.Day; day++)
                            {
                                DateTime fecha = new DateTime(fechaSalida.Year, fechaSalida.Month, day);
                                // Si es lunes a viernes
                                if (fecha.DayOfWeek != DayOfWeek.Saturday && fecha.DayOfWeek != DayOfWeek.Sunday)
                                {
                                    diasTrabajados++;
                                }
                            }
                            salarioPendiente = diasTrabajados * salarioDiario;
                            totalPagar = aguinaldo + vacaciones + salarioPendiente;
                            cesantia = 0;
                            preavisoDinero = 0;

                        } else
                        {
                            var salarioDiario = salarioMesual / 30;
                            int diasTrabajados = 0;
                            for (int day = 1; day <= fechaSalida.Day; day++)
                            {
                                DateTime fecha = new DateTime(fechaSalida.Year, fechaSalida.Month, day);
                                // Si es lunes a viernes
                                if (fecha.DayOfWeek != DayOfWeek.Saturday && fecha.DayOfWeek != DayOfWeek.Sunday)
                                {
                                    diasTrabajados++;
                                }
                            }
                            salarioPendiente = diasTrabajados * salarioDiario;
                            totalPagar = cesantia + aguinaldo + vacaciones + preavisoDinero + salarioPendiente;
                        }









                        List<liquidacion> liqui = new List<liquidacion>();

                        liquidacion liquidacionNueva = new liquidacion
                        {
                            descripcion = DateTime.Now.ToString(),
                            monto_pagar = totalPagar,
                            tipo_Liquidacion = tipo_Liquidacion,
                            id_Persona = persona.id_Persona,
                            vacacionesNoDistrutadas = vacaciones,
                            cesantia = cesantia,
                            aguinaldo = aguinaldo,
                            preaviso = preavisoDinero,
                            diasTrabajados = salarioPendiente

                        };

                        var respuestaGuradar = objCapaDato.registrar(liquidacionNueva, out mensaje);
                        if (respuestaGuradar != 0) {
                            var resp = objCapaDato.actualizarPersonaSalida(DateTime.Now, persona.id_Persona);

                        }

                        liqui.Add(liquidacionNueva);

                        calculado = liqui;
                    }





                } 
              
               
                return calculado;
            }
            catch (Exception)
            {

                throw;
            }
            




         
        }















    }
}
