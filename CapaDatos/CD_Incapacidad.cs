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
    public class CD_Incapacidad
    {


        public List<incapacidad> listar()
        {



            List<incapacidad> lista = new List<incapacidad>();


            try
            {

                using (SqlConnection con = new SqlConnection(conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand(@"SELECT i.*,p.nombre as nombre,p.apellido
                                                     FROM incapacidad i
                                                     JOIN persona p ON i.id_Persona = p.id_Persona", con);
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new incapacidad
                            {
                                id_Incapacidad = Convert.ToInt32(dr["id_incapacidad"]),
                                tipo_Incapacidad = dr["tipo_Incapacidad"].ToString(),
                                fecha_Finalizacion= dr["fecha_Finalizacion"].ToString(),
                                fecha_Inicio = dr["fecha_Inicio"].ToString(),
                                fecha_Solicitud = dr["fecha_Solicitud"].ToString(),
                                monto_Apagar = Convert.ToDecimal(dr["monto_Apagar"]),
                                descripcion = dr["descripcion"].ToString(),
                                id_Persona= Convert.ToInt32(dr["id_persona"]),
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

                lista = new List<incapacidad>();
            }




            return lista;
        }

        public int registrar(incapacidad obj, out string mensaje)
        {
            

            int id_Autogenerado = 0;

            mensaje = string.Empty;

            try
            {

                using (SqlConnection con = new SqlConnection(conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarIncapacidad", con);
                    cmd.Parameters.AddWithValue("tipo_Incapacidad", obj.tipo_Incapacidad);
                    cmd.Parameters.AddWithValue("fecha_Inicio", obj.fecha_Inicio); // Correcto
                    cmd.Parameters.AddWithValue("fecha_Solicitud", obj.fecha_Solicitud);
                    cmd.Parameters.AddWithValue("fecha_Finalizacion", obj.fecha_Finalizacion);

                    cmd.Parameters.AddWithValue("monto_Apagar", obj.monto_Apagar);
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



        public Boolean editar(incapacidad obj, out string mensaje)
        {


            bool resultado = false;

            mensaje = string.Empty;

            try
            {

                using (SqlConnection con = new SqlConnection(conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarIncapacidad", con);
                    cmd.Parameters.AddWithValue("id_Incapacidad", obj.id_Incapacidad);
                    cmd.Parameters.AddWithValue("tipo_Incapacidad", obj.tipo_Incapacidad);
                    cmd.Parameters.AddWithValue("fecha_Inicio", obj.fecha_Inicio);
                    cmd.Parameters.AddWithValue("fecha_Solicitud", obj.fecha_Solicitud);
                    cmd.Parameters.AddWithValue("fecha_Finalizacion", obj.fecha_Finalizacion);
                    cmd.Parameters.AddWithValue("monto_Apagar", obj.monto_Apagar);
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
                mensaje = "No se pudo guardar los cambios. Por favor, verifica los datos ingresados. " + ex.Message;
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
                    SqlCommand cmd = new SqlCommand("delete top(1) from incapacidad where id_incapacidad =@id", oconexion);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }


            }
            catch (Exception ex)
            {

                resultado = false;
                mensaje = "No se pudo guardar los cambios. Por favor, verifica los datos ingresados. ";
            }
            return resultado;












        }



        public bool aprobacion(int id_Persona)
        {

            bool resultado = false;


            try
            {

                using (SqlConnection con = new SqlConnection(conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("UPDATE incapacidad set aprobado=1 where id_Persona = @id_Persona", con);
                    cmd.Parameters.AddWithValue("@id_Persona", id_Persona);

                    cmd.CommandType = CommandType.Text;


                    con.Open();
                    cmd.ExecuteNonQuery();


                    resultado = true;

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