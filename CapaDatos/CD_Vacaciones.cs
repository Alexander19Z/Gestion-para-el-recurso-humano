using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace CapaDatos
{
    public class CD_Vacaciones
    {

            public List<vacaciones> listar()
            {



                List<vacaciones> lista = new List<vacaciones>();


                try
                {

                    using (SqlConnection con = new SqlConnection(conexion.cn))
                    {
                        SqlCommand cmd = new SqlCommand(@"SELECT v.*,p.nombre as nombre, p.apellido as apellido
                                                       FROM vacaciones v
                                                       JOIN persona p on p.id_Persona= v.id_Persona", con);

                        cmd.CommandType = CommandType.Text;
                        con.Open();
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {

                                lista.Add(new vacaciones
                                {
                                    id_Persona = Convert.ToInt32(dr["id_Persona"]),
                                    saldo_Vacaciones = Convert.ToDecimal(dr["saldo_Vacaciones"]),
                                    fecha_Modificacion= dr["fecha_Modificacion"].ToString(),
                                    dias_Ganado = Convert.ToDecimal(dr["dias_Ganado"]),
                                    dias_Solicitado = Convert.ToDecimal(dr["dias_Solicitado"]),
                                    monto_Total = Convert.ToDecimal(dr["monto_Total"]),
                                    nombre= dr["nombre"].ToString(),
                                    apellido= dr["apellido"].ToString()

                                });

                            }

                        }

                    }

                }
                catch (Exception)
                {

                    lista = new List<vacaciones>();
                }




                return lista;
            }

            public int registrar(vacaciones obj, out string mensaje)
            {


            int id_Autogenerado = 0;

            mensaje = string.Empty;

                try
                {

                    using (SqlConnection con = new SqlConnection(conexion.cn))
                    {
                        SqlCommand cmd = new SqlCommand("sp_RegistrarVacaciones", con);
                        cmd.Parameters.AddWithValue("id_Persona", obj.id_Persona);
                        cmd.Parameters.AddWithValue("saldo_Vacaciones", obj.saldo_Vacaciones);
                        cmd.Parameters.AddWithValue("dias_Ganado", obj.dias_Ganado);
                        cmd.Parameters.AddWithValue("dias_Solicitado", obj.dias_Solicitado);
                        cmd.Parameters.AddWithValue("monto_Total", obj.monto_Total);
                        cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        cmd.CommandType = CommandType.StoredProcedure;


                        con.Open();
                        cmd.ExecuteNonQuery();
                    id_Autogenerado = 1;
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



            public Boolean editar(vacaciones obj, out string mensaje)
            {


                bool resultado = false;

                mensaje = string.Empty;

                try
                {

                    using (SqlConnection con = new SqlConnection(conexion.cn))
                    {
                        SqlCommand cmd = new SqlCommand("sp_EditarVacaciones", con);
                    cmd.Parameters.AddWithValue("id_Persona", obj.id_Persona);
                    cmd.Parameters.AddWithValue("saldo_Vacaciones", obj.saldo_Vacaciones);
                    cmd.Parameters.AddWithValue("dias_Ganado", obj.dias_Ganado);
                    cmd.Parameters.AddWithValue("dias_Solicitado", obj.dias_Solicitado);
                    cmd.Parameters.AddWithValue("monto_Total", obj.monto_Total);

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
                        SqlCommand cmd = new SqlCommand("delete top(1) from vacaciones where id_Persona =@id", oconexion);
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





        public List<vacaciones> saldos(int id)
        {
            List<vacaciones> lista = new List<vacaciones>();
            try
            {

                using (SqlConnection con = new SqlConnection(conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("select saldo_Vacaciones,dias_Solicitado,monto_Total from vacaciones where id_Persona=@id", con);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            lista.Add(new vacaciones
                            {
                                saldo_Vacaciones = Convert.ToDecimal(dr["saldo_Vacaciones"]),
                                dias_Solicitado = Convert.ToDecimal(dr["dias_Solicitado"]),
                                monto_Total = Convert.ToDecimal(dr["monto_Total"])

                            });

                        }

                    }

                }

            }
            catch (Exception)
            {

                lista = new List<vacaciones>();
            }

            return lista;
        }


        public int actualizarVacaciones(string nombre,decimal cantidadDias) {

            try
            {
                int filasModificadas = 0;
                using (SqlConnection con = new SqlConnection(conexion.cn))
                {

                    SqlCommand cmd = new SqlCommand(@" UPDATE v
                                                          SET v.fecha_Modificacion = CONVERT(VARCHAR, GETDATE(), 103), 
                                                          v.dias_Solicitado = v.dias_Solicitado + @cantidadDias,
                                                          v.saldo_Vacaciones = v.saldo_Vacaciones - @cantidadDias
                                                          FROM vacaciones v
                                                          INNER JOIN persona p ON v.id_Persona = p.id_Persona
                                                          WHERE p.Nombre =@nombre;", con);
                    con.Open();

                    cmd.Parameters.AddWithValue("@cantidadDias", cantidadDias);
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandType = CommandType.Text;

                    filasModificadas = cmd.ExecuteNonQuery();
                        

                    




                }






                return filasModificadas;


            }
            catch (Exception ex)
            {

                return 0;
            }














           
        }


        public Boolean MontoEnDineroVacaciones(int persona,decimal salarioPorDia)
        {


            bool resultado = false;

            

            try
            {
                using (SqlConnection con = new SqlConnection(conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand(@"update vacaciones
                                                    set monto_Total = dias_Ganado*@salarioPorDia
                                                    where id_Persona= @persona", con);
                   


                    con.Open();
                    cmd.Parameters.AddWithValue("@salarioPorDia", salarioPorDia);
                    cmd.Parameters.AddWithValue("@persona",persona);
                    cmd.CommandType = CommandType.Text;


                   int res= cmd.ExecuteNonQuery();
                    if (res < 0)
                    {

                        resultado = true;
                    }
                    else { resultado = false;
                    }
                }

                

            }
            catch (Exception ex)
            {

                resultado = false;
               
            }
            return resultado;

        }



    }



}
