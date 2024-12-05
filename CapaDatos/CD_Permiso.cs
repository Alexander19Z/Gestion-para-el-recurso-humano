using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Diagnostics.Eventing.Reader;

namespace CapaDatos
{
    public class CD_Permiso
    {
        public List<permiso> listar()
        {



            List<permiso> lista = new List<permiso>();


            try
            {

                using (SqlConnection con = new SqlConnection(conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand(@"SELECT e.*,p.nombre as nombre,p.apellido
                                                    FROM permiso e
                                                    JOIN persona p ON e.id_Persona = p.id_Persona", con);
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                           
                            lista.Add(new permiso
                            {
                                id_Permiso = Convert.ToInt32(dr["id_Permiso"]),
                                fecha_Solicitud = dr["fecha_Solicitud"].ToString(),
                                hora_Inicio =dr["hora_Inicio"].ToString(),
                                hora_Finalizacion =dr["hora_Finalizacion"].ToString(),
                                concepto = dr["concepto"].ToString(),
                                descripcion = dr["descripcion"].ToString(),
                                id_Persona = Convert.ToInt32(dr["id_persona"]),
                                aprobado = Convert.ToBoolean(dr["aprobado"]),
                                nombre = dr["nombre"].ToString(),
                                apellido = dr["apellido"].ToString()



                            });

                        }

                    }

                }

            }
            catch (Exception)
            {

                lista = new List<permiso>();
            }




            return lista;
        }

        public int registrar(permiso obj, out string mensaje)
        {


            int id_Autogenerado = 0;

            mensaje = string.Empty;

            try
            {

                using (SqlConnection con = new SqlConnection(conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarPermiso", con);
                    cmd.Parameters.AddWithValue("fecha_Solicitud", obj.fecha_Solicitud);
                    cmd.Parameters.AddWithValue("hora_Inicio", obj.hora_Inicio);
                    cmd.Parameters.AddWithValue("hora_Finalizacion", obj.hora_Finalizacion);
                    cmd.Parameters.AddWithValue("concepto", obj.concepto);
                    cmd.Parameters.AddWithValue("descripcion", obj.descripcion);
                    cmd.Parameters.AddWithValue("id_Persona", obj.id_Persona);
                    cmd.Parameters.AddWithValue("aprobado", obj.aprobado);
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



        public Boolean editar(permiso obj, out string mensaje)
        {


            bool resultado = false;

            mensaje = string.Empty;

            try
            {

                using (SqlConnection con = new SqlConnection(conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarPermiso", con);
                    cmd.Parameters.AddWithValue("id_Permiso", obj.id_Permiso);
                    cmd.Parameters.AddWithValue("fecha_Solicitud", obj.fecha_Solicitud);
                    cmd.Parameters.AddWithValue("hora_Inicio", obj.hora_Inicio);
                    cmd.Parameters.AddWithValue("hora_Finalizacion", obj.hora_Finalizacion);
                    cmd.Parameters.AddWithValue("concepto", obj.concepto);
                    cmd.Parameters.AddWithValue("descripcion", obj.descripcion);
                    cmd.Parameters.AddWithValue("id_Persona", obj.id_Persona);
                    cmd.Parameters.AddWithValue("aprobado", obj.aprobado);
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
                    SqlCommand cmd = new SqlCommand("delete top(1) from permiso where id_Permiso =@id", oconexion);
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
        //Se quiere crear un metodo que  haga y verifique si tiene un permo sin goce de salior que se le tenga reducir 
        public bool verificarPermiso() { return false; }



       





    }
}
