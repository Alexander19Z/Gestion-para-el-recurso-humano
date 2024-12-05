using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections;
using System.Globalization;

namespace CapaDatos
{
    public class CD_Planilla
    {
        public List<planilla> listar()
        {



            List<planilla> lista = new List<planilla>();


            try
            {

                using (SqlConnection con = new SqlConnection(conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand(@"SELECT pl.*, p.nombre as nombre, p.apellido as apellido
                                                    FROM planilla pl
                                                    JOIN persona p on p.id_Persona = pl.id_Persona", con);
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new planilla
                            {
                                id_Planilla = Convert.ToInt32(dr["id_Planilla"]),
                                id_Persona = Convert.ToInt32(dr["id_Persona"]),
                                salario_Total = Convert.ToDecimal(dr["salario_Total"]),
                                deduccion_CCSS = Convert.ToDecimal(dr["deduccion_CCSS"]),
                                deduccion_permiso = Convert.ToDecimal(dr["deduccion_permiso"]),
                                deduccion_InpuestoRenta = Convert.ToDecimal(dr["deduccion_InpuestoRenta"]),
                                deduccion_Incapacidad = Convert.ToDecimal(dr["deduccion_Incapacidad"]),
                                salario_Pagar = Convert.ToDecimal(dr["salario_Pagar"]),
                                fecha_creacion = Convert.ToDateTime(dr["fecha_creacion"]).ToString("yyyy-MM-dd"),
                                fecha_Pago =Convert.ToDateTime(dr["fecha_Pago"]).ToString("yyyy-MM"),
                                deducciones_Varias = Convert.ToDecimal(dr["deducciones_Varias"]),
                                nombre = dr["nombre"].ToString(),
                                apellido = dr["apellido"].ToString()

                            }); ;

                        }

                    }

                }

            }
            catch (Exception)
            {

                lista = new List<planilla>();
            }




            return lista;
        }

        public List<ultimosTresPagos> ultimosTresSueldos(int idPersona)
        {
            List<ultimosTresPagos> lisPago = new List<ultimosTresPagos>();
            string mensaje = string.Empty;
            try
            {

                using (SqlConnection con = new SqlConnection(conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand(@"WITH UltimosPagos AS (
                                                     SELECT 
                                                        id_Persona, 
                                                        salario_Pagar, 
                                                        fecha_Pago,
                                                      ROW_NUMBER() OVER (PARTITION BY id_Persona ORDER BY fecha_Pago DESC) AS rn
                                                     FROM planilla WHERE id_Persona =@idPersona
                                                                            )
                                                    SELECT id_Persona, salario_Pagar, fecha_Pago
                                                    FROM UltimosPagos
                                                    WHERE rn <= 3
                                                    ORDER BY id_Persona, fecha_Pago DESC; ", con);

                    cmd.Parameters.AddWithValue("@IdPersona", idPersona);
                    con.Open();
                    using (SqlDataReader re = cmd.ExecuteReader())
                    {

                        while (re.Read())
                        {
                            lisPago.Add(new ultimosTresPagos
                            {
                                IdPersona = Convert.ToInt32(re["id_Persona"]),
                                SalarioPagar = Convert.ToDecimal(re["salario_Pagar"]),
                                FechaPago = re["fecha_Pago"].ToString()
                            });
                        }

                    }


                }

                return lisPago;
            }
            catch (Exception ex)
            {

                mensaje = "No se pudo guardar los cambios. Por favor, verifica los datos ingresados. ";
                return lisPago = new List<ultimosTresPagos>();
            }
        }


        public int registrar(planilla obj, out string mensaje)
        {


            int id_Autogenerado = 0;

            mensaje = string.Empty;
            var fechaPago = obj.fecha_Pago.Replace("-", "/").ToString();
            try
            {

                using (SqlConnection con = new SqlConnection(conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarPlanilla", con);
                    cmd.Parameters.AddWithValue("id_Persona", obj.id_Persona);
                    cmd.Parameters.AddWithValue("salario_Total", obj.salario_Total);
                    cmd.Parameters.AddWithValue("deduccion_CCSS", obj.deduccion_CCSS);
                    cmd.Parameters.AddWithValue("deduccion_InpuestoRenta", obj.deduccion_InpuestoRenta);
                    cmd.Parameters.AddWithValue("deduccion_permiso", obj.deduccion_permiso);
                    cmd.Parameters.AddWithValue("deduccion_Incapacidad", obj.deduccion_Incapacidad);
                    cmd.Parameters.AddWithValue("salario_Pagar",obj.salario_Pagar);
                    cmd.Parameters.AddWithValue("fecha_creacion", FormatearFecha(obj.fecha_creacion, true));
                    cmd.Parameters.AddWithValue("fecha_Pago", FormatearFecha(obj.fecha_Pago,false));
                    cmd.Parameters.AddWithValue("deducciones_Varias", obj.deducciones_Varias);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    id_Autogenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    mensaje = cmd.Parameters["mensaje"].Value.ToString();

                }

            }
            catch (Exception ex)
            {

                id_Autogenerado = 0;
                mensaje = "No se pudo guardar los cambios. Por favor, verifica los datos ingresados. ";
            }
            return id_Autogenerado;

        }


        public Boolean editar(planilla obj, out string mensaje)
        {


            bool resultado = false;

            mensaje = string.Empty;

            try
            {

                using (SqlConnection con = new SqlConnection(conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarPlanilla", con);
                    cmd.Parameters.AddWithValue("id_Planilla", obj.id_Planilla);
                    cmd.Parameters.AddWithValue("id_Persona", obj.id_Persona);
                    cmd.Parameters.AddWithValue("salario_Total", obj.salario_Total);
                    cmd.Parameters.AddWithValue("deduccion_CCSS", obj.deduccion_CCSS);
                    cmd.Parameters.AddWithValue("deduccion_InpuestoRenta", obj.deduccion_InpuestoRenta);
                    cmd.Parameters.AddWithValue("deduccion_Incapacidad", obj.deduccion_Incapacidad);
                    cmd.Parameters.AddWithValue("deduccion_permiso", obj.deduccion_permiso);
                     cmd.Parameters.AddWithValue("salario_Pagar",obj.salario_Pagar);
                    cmd.Parameters.AddWithValue("fecha_creacion",  Convert.ToDateTime(obj.fecha_creacion).ToString("dd/MM/yyyy"));
                    cmd.Parameters.AddWithValue("fecha_Pago", Convert.ToDateTime(obj.fecha_Pago).ToString("MM/yyyy"));
                    cmd.Parameters.AddWithValue("deducciones_Varias", obj.deducciones_Varias);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;


                    con.Open();
                    cmd.ExecuteNonQuery();
                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    mensaje = cmd.Parameters["Mensaje"].Value.ToString();



                }

            }
            catch (Exception ex)
            {

                resultado = false;
                mensaje = ex.Message;
            }
            return resultado;

        }




        public bool eliminar(int id, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;
            try
            {

                using (SqlConnection oconexion = new SqlConnection(conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("delete top(1) from planilla where id_Planilla =@id", oconexion);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }


            }
            catch (Exception ex)
            {

                resultado = false;
                mensaje = ex.Message;
            }
            return resultado;
        }



        public Boolean calculoPlanilla()
        {

            var resultado = false;
            try
            {

                using (SqlConnection oconexion = new SqlConnection(conexion.cn))
                {
                    oconexion.Open();

                    using (SqlCommand cmd = new SqlCommand("sp_CalcularPlanilla", oconexion))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();


                        resultado = true;
                    }


                    oconexion.Close();

                }






                return resultado;


            }
            catch (Exception)
            {
                resultado = false;

            }

            return resultado;
        }



        public bool actualizarPermiso(int id_Persona, decimal nuevo_Valor,string fecha_Pago, out string mensaje)
        {
            mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(conexion.cn))
                {

                    SqlCommand cmd = new SqlCommand(@"UPDATE planilla
                                                    SET deduccion_permiso =@nuevo_Valor,
                                                    salario_Pagar=salario_Pagar - @nuevo_Valor
                                                    WHERE id_Persona=@id_Persona AND  fecha_Pago =@fecha_Pago ", oconexion);
                    cmd.Parameters.AddWithValue("@nuevo_Valor", nuevo_Valor);
                    cmd.Parameters.AddWithValue("@id_Persona", id_Persona);
                    cmd.Parameters.AddWithValue("@fecha_Pago", Convert.ToDateTime(fecha_Pago).ToString("MM/yyyy"));
                    oconexion.Open();
                    int lineasAfectadas = cmd.ExecuteNonQuery();

                    if (lineasAfectadas > 0)
                    {
                        return true;

                    }
                    else { return false; }

                }



            }
            catch (Exception ex)
            {
                mensaje = "Ocurrió un error: " + ex.Message;
                return false;
            }





        }


        public bool actualizarIncapacidad(int idPersona, decimal saldoIncapacidad,string fecha_Pago, out string mensaje)
        {
            try
            {
                mensaje = string.Empty;

                using (SqlConnection oconexion = new SqlConnection(conexion.cn))
                {

                    SqlCommand cmd = new SqlCommand(@"UPDATE planilla
                                                    SET deduccion_Incapacidad =@nuevo_Valor,
                                                        deduccion_CCSS=0,
                                                        deduccion_permiso=0,
                                                        deduccion_InpuestoRenta=0,
                                                    salario_Pagar=salario_Total - @nuevo_Valor - deduccion_Prestamo
                                                    WHERE id_Persona=@id_Persona AND fecha_Pago=@fecha_Pago", oconexion);
                    cmd.Parameters.AddWithValue("nuevo_Valor", saldoIncapacidad);
                    cmd.Parameters.AddWithValue("id_Persona", idPersona);
                    cmd.Parameters.AddWithValue("fecha_Pago", fecha_Pago);
                    oconexion.Open();
                    int lineasAfectadas = cmd.ExecuteNonQuery();

                    if (lineasAfectadas > 0)
                    {
                        return true;

                    }
                    else { return false; }

                }
            }
            catch (Exception ex)
            {

                mensaje = "Ocurrió un error: " + ex.Message;
                return false;
            }
        }


        public bool actualizarIncapacidadEnfermedad(int idPersona, decimal saldoIncapacidad, string fecha_Pago,decimal totalPagarIncapacidad, out string mensaje)
        {
            try
            {
                mensaje = string.Empty;
                using (SqlConnection oconexion = new SqlConnection(conexion.cn))
                {

                    SqlCommand cmd = new SqlCommand(@"UPDATE planilla
                                                    SET deduccion_Incapacidad =deduccion_Incapacidad+@nuevo_Valor,
                                                    salario_Pagar=(salario_Pagar - @nuevo_Valor)+@nuevo_Sumar
                                                   
                                                    WHERE id_Persona=@id_Persona  AND fecha_Pago=@fecha_Pago", oconexion);
                    cmd.Parameters.AddWithValue("nuevo_Valor", saldoIncapacidad);
                    cmd.Parameters.AddWithValue("nuevo_Sumar", totalPagarIncapacidad);
                    cmd.Parameters.AddWithValue("id_Persona", idPersona);
                    cmd.Parameters.AddWithValue("fecha_Pago", fecha_Pago);
                    oconexion.Open();
                    int lineasAfectadas = cmd.ExecuteNonQuery();

                    if (lineasAfectadas > 0)
                    {
                        return true;

                    }
                    else { return false; }

                }



            }
            catch (Exception ex)
            {
                mensaje = "Ocurrió un error: " + ex.Message;
                return false;
            }

        }



        public bool actualizarPension(int id_Persona, decimal nuevo_Valor, string fecha_Pago, out string mensaje)
        {
            mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(conexion.cn))
                {

                    SqlCommand cmd = new SqlCommand(@"UPDATE planilla
                                                    SET deducciones_Varias =@nuevo_Valor,
                                                    salario_Pagar=salario_Pagar - @nuevo_Valor
                                                    WHERE id_Persona=@id_Persona  AND fecha_Pago=@fecha_Pago", oconexion);
                    cmd.Parameters.AddWithValue("nuevo_Valor", nuevo_Valor);
                    cmd.Parameters.AddWithValue("id_Persona", id_Persona);
                    cmd.Parameters.AddWithValue("fecha_Pago", fecha_Pago);
                    oconexion.Open();
                    int lineasAfectadas = cmd.ExecuteNonQuery();

                    if (lineasAfectadas > 0)
                    {
                        return true;

                    }
                    else { return false; }

                }



            }
            catch (Exception ex)
            {
                mensaje = "Ocurrió un error: " + ex.Message;
                return false;
            }





        }


        public bool actualizarIngresoNuevo(int id_Persona, decimal nuevo_Valor, decimal CCSS,decimal salario_Total ,string fecha_Pago, out string mensaje)
        {
            mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(conexion.cn))
                {

                    SqlCommand cmd = new SqlCommand(@"UPDATE planilla
                                                    SET salario_Total =@salario_Total ,
                                                    salario_Pagar= @nuevo_Valor-@CCSS,
                                                    deduccion_CCSS=@CCSS,
                                                    deduccion_InpuestoRenta=0
                                                    WHERE id_Persona=@id_Persona  AND fecha_Pago=@fecha_Pago", oconexion);
                    cmd.Parameters.AddWithValue("nuevo_Valor", nuevo_Valor);
                    cmd.Parameters.AddWithValue("id_Persona", id_Persona);
                    cmd.Parameters.AddWithValue("fecha_Pago", fecha_Pago);
                    cmd.Parameters.AddWithValue("salario_Total", salario_Total);
                    cmd.Parameters.AddWithValue("CCSS", CCSS);
                    oconexion.Open();
                    int lineasAfectadas = cmd.ExecuteNonQuery();

                    if (lineasAfectadas > 0)
                    {
                        return true;

                    }
                    else { return false; }

                }



            }
            catch (Exception ex)
            {
                mensaje = "Ocurrió un error: " + ex.Message;
                return false;
            }





        }






        public bool actualizarDeducciones(int id_Persona,int id_Deducciones, out string mensaje)
        {
            mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(conexion.cn))
                {

                    SqlCommand cmd = new SqlCommand(@"UPDATE deducciones
                                                    SET cantidad_Cuotas=cantidad_Cuotas+1
                                                    WHERE id_Persona=@id_Persona AND id_Deduccion= @id_Deducciones", oconexion);
                    
                    cmd.Parameters.AddWithValue("id_Persona", id_Persona);
                    cmd.Parameters.AddWithValue("id_Deducciones", id_Deducciones);

                    oconexion.Open();
                    int lineasAfectadas = cmd.ExecuteNonQuery();

                    if (lineasAfectadas > 0)
                    {
                        return true;

                    }
                    else { return false; }

                }



            }
            catch (Exception ex)
            {
                mensaje = "Ocurrió un error: " + ex.Message;
                return false;
            }





        }


        public static string FormatearFecha(string fechaOriginal, bool incluirDia)
        {
            try
            {
                if (incluirDia)
                {
                    DateTime fecha = DateTime.ParseExact(fechaOriginal, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    return fecha.ToString("dd/MM/yyyy"); 
                }
                else
                {
                    
                    DateTime fecha = DateTime.ParseExact(fechaOriginal, "yyyy-MM", CultureInfo.InvariantCulture);
                    return fecha.ToString("MM/yyyy"); 
                }
            }
            catch (FormatException)
            {
                return "Formato de fecha no válido";
            }
        }
    }








}

