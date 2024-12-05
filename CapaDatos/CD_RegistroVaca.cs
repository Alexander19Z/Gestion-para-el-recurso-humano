using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_RegistroVaca
    {


        public List<registro_Vaca> listar()
        {



            List<registro_Vaca> lista = new List<registro_Vaca>();


            try
            {

                using (SqlConnection con = new SqlConnection(conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand(@"SELECT pr.*,p.nombre as nombre, p.apellido as apellido 
                                                    FROM registro_Vaca pr
                                                    JOIN persona p on p.id_Persona= pr.id_Persona", con);
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            lista.Add(new registro_Vaca
                            {
                                id_Registro = Convert.ToInt32(dr["id_Registro"]),
                                tipo_Registro = Convert.ToChar(dr["tipo_Registro"]),
                                id_Persona = Convert.ToInt32(dr["id_Persona"]),
                                fecha_Registro= dr["fecha_Registro"].ToString(),
                                tipo_Movimiento= dr["tipo_Movimiento"].ToString(),
                                periodo= dr["periodo"].ToString(),
                                nombre = dr["nombre"].ToString(),
                                apellido = dr["apellido"].ToString()

                            });

                        }

                    }

                }

            }
            catch (Exception)
            {

                lista = new List<registro_Vaca>();
            }




            return lista;
        }

        public int registrar(registro_Vaca obj, out string mensaje)
        {


            int id_Autogenerado = 0;

            mensaje = string.Empty;

            try
            {

                using (SqlConnection con = new SqlConnection(conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarRegiVaca", con);
                    cmd.Parameters.AddWithValue("tipo_Registro", obj.tipo_Registro);
                    cmd.Parameters.AddWithValue("id_Persona", obj.id_Persona);
                    cmd.Parameters.AddWithValue("fecha_Registro", obj.fecha_Registro);
                    cmd.Parameters.AddWithValue("tipo_Movimiento", obj.tipo_Movimiento);
                    cmd.Parameters.AddWithValue("periodo", obj.periodo);
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



        public Boolean editar(registro_Vaca obj, out string mensaje)
        {


            bool resultado = false;

            mensaje = string.Empty;

            try
            {

                using (SqlConnection con = new SqlConnection(conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarRegiVaciones", con);
                    cmd.Parameters.AddWithValue("id_Registro", obj.id_Registro);
                    cmd.Parameters.AddWithValue("tipo_Registro", obj.tipo_Registro);
                    cmd.Parameters.AddWithValue("id_Persona", obj.id_Persona);
                    cmd.Parameters.AddWithValue("fecha_Registro", obj.fecha_Registro);
                    cmd.Parameters.AddWithValue("tipo_Movimiento", obj.tipo_Movimiento);
                    cmd.Parameters.AddWithValue("periodo", obj.periodo);

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
                    SqlCommand cmd = new SqlCommand("delete top(1) from registro_Vaca where id_Registro =@id", oconexion);
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







    }
























}

