using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Diagnostics;


namespace CapaDatos
{
    public class CD_Aguinaldo
    {

        public List<aguinaldo> listar()
        {



            List<aguinaldo> lista = new List<aguinaldo>();


            try
            {

                using (SqlConnection con = new SqlConnection(conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand(@"SELECT pr.*,p.nombre as nombre, p.apellido as apellido 
                                                    FROM aguinaldo pr
                                                    JOIN persona p on p.id_Persona= pr.id_Persona", con);
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new aguinaldo
                            {
                                id_Aguinaldo= Convert.ToInt32(dr["id_Aguinaldo"]),
                                monto_Total =Convert.ToDecimal(dr["monto_Total"]),
                                fecha_Pago = Convert.ToDateTime(dr["fecha_Pago"]),
                                id_Persona= Convert.ToInt32(dr["id_Persona"]),
                                nombre = dr["nombre"].ToString(),
                                apellido = dr["apellido"].ToString()


                            }); ;

                        }

                    }

                }

            }
            catch (Exception)
            {

                lista = new List<aguinaldo>();
            }




            return lista;
        }

        public int registrar(aguinaldo obj, out string mensaje)
        {


            int id_Autogenerado = 0;

            mensaje = string.Empty;

            try
            {

                using (SqlConnection con = new SqlConnection(conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarAguinaldo", con);
                        cmd.Parameters.AddWithValue("monto_Total", obj.monto_Total);
                        cmd.Parameters.AddWithValue("fecha_Pago", obj.fecha_Pago);
                        cmd.Parameters.AddWithValue("id_Persona", obj.id_Persona);
                        cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        cmd.CommandType = CommandType.StoredProcedure;


                        con.Open();
                        cmd.ExecuteNonQuery();
                        id_Autogenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                        mensaje = cmd.Parameters["mensaje"].Value.ToString();



                }

            }
            catch (SqlException ex)
            {
                
                LogError(ex); 

                mensaje = "No se pudo guardar los cambios. Por favor, verifica los datos ingresados.";
            }
            catch (Exception ex)
            {

                id_Autogenerado = 0;
                mensaje = "No se pudo guardar los cambios. Por favor, verifica los datos ingresados. " ;
            }
            return id_Autogenerado;

        }



        public Boolean editar(aguinaldo obj, out string mensaje)
        {


            bool resultado = false;

            mensaje = string.Empty;

            try
            {

                using (SqlConnection con = new SqlConnection(conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarAguinaldo", con);
                    cmd.Parameters.AddWithValue("id_Aguinaldo", obj.id_Aguinaldo);
                    cmd.Parameters.AddWithValue("monto_Total", obj.monto_Total);
                    cmd.Parameters.AddWithValue("fecha_Pago", obj.fecha_Pago);
                    cmd.Parameters.AddWithValue("id_Persona", obj.id_Persona);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;


                    con.Open();
                    cmd.ExecuteNonQuery();
                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    mensaje = cmd.Parameters["Mensaje"].Value.ToString();



                }

            }
            catch (SqlException ex)
            {

                LogError(ex);

                mensaje = "No se pudo guardar los cambios. Por favor, verifica los datos ingresados.";
            }
            catch (Exception ex)
            {

                resultado = false;
                mensaje = "No se pudo guardar los cambios. Por favor, verifica los datos ingresados. ";
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
                    SqlCommand cmd = new SqlCommand("delete top(1) from aguinaldo where id_Aguinaldo =@id", oconexion);
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



        public bool eliminarAguinaldoCalulado()
        {
            bool resultado = false;
            var ano= DateTime.Now.Year;
           
            try
            {

                using (SqlConnection oconexion = new SqlConnection(conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand(@"delete from aguinaldo 
                                                    where YEAR(CAST(fecha_Pago AS DATE)) = @anio_actual", oconexion);
                    cmd.Parameters.AddWithValue("@anio_actual", ano);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }


            }
            catch (Exception ex)
            {

                resultado = false;
              
            }
            return resultado;
        }







        public List<aguinaldoMeses> listarMeses()
        {



            List<aguinaldoMeses> lista = new List<aguinaldoMeses>();


            try
            {

                using (SqlConnection con = new SqlConnection(conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("ObtenerPagosMensuales", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new aguinaldoMeses
                            {
                                id_Persona = Convert.ToInt32(dr["id_Persona"]),
                                Diciembre_Anterior = Convert.ToDecimal(dr["Diciembre_Anterior"]),
                                Enero = Convert.ToDecimal(dr["Enero"]),
                                Febrero = Convert.ToDecimal(dr["Febrero"]),
                                Marzo = Convert.ToDecimal(dr["Marzo"]),
                                Abril = Convert.ToDecimal(dr["Abril"]),
                                Mayo = Convert.ToDecimal(dr["Mayo"]),
                                Junio = Convert.ToDecimal(dr["Junio"]),
                                Julio = Convert.ToDecimal(dr["Julio"]),
                                Agosto = Convert.ToDecimal(dr["Agosto"]),
                                Septiembre = Convert.ToDecimal(dr["Septiembre"]),
                                Octubre = Convert.ToDecimal(dr["Octubre"]),
                                Noviembre = Convert.ToDecimal(dr["Noviembre"]),
                                TotalAnio = Convert.ToDecimal(dr["TotalAnio"]),
                                
                                aguinaldoPagar = Convert.ToDecimal(dr["Diciembre_Anterior"]),
                                nombre = dr["nombre"].ToString(),
                                apellido = dr["apellido"].ToString()
                            }); ;

                        }

                    }

                }

            }
            catch (Exception)
            {

                lista = new List<aguinaldoMeses>();
            }




            return lista;
        }
        private void LogError(Exception ex)
        {
            string logPath = @"C:\Logs\app_log.txt";
            string errorDetails = $"{DateTime.Now}: {ex.Message} - {ex.StackTrace}\n";

            // Crear el archivo si no existe, o agregar contenido si ya existe
            System.IO.File.AppendAllText(logPath, errorDetails);
        }


    }
}
