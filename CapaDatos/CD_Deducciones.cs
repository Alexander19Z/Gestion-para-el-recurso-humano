using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;

namespace CapaDatos
{
    public class CD_Deducciones
    {

        public List<deducciones> listar()
        {



            List<deducciones> lista = new List<deducciones>();


            try
            {

                using (SqlConnection con = new SqlConnection(conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand(@"SELECT pr.*,p.nombre as nombre, p.apellido as apellido
                                                       FROM deducciones pr
                                                       JOIN persona p on p.id_Persona= pr.id_Persona", con);

                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            lista.Add(new deducciones
                            {
                                id_Deduccion = Convert.ToInt32(dr["id_Deduccion"]),
                                tipo_Deduccion= dr["tipo_Deduccion"].ToString(),
                                cantidad_Cuotas = Convert.ToDecimal(dr["cantidad_Cuotas"]),
                                fecha_Inicio = dr["fecha_Inicio"].ToString(),
                                fecha_Finalizacion = dr["fecha_Finalizacion"].ToString(),
                                monto_Cuota = Convert.ToDecimal(dr["monto_Cuota"]),
                                activo= Convert.ToBoolean(dr["activo"]),
                                id_Persona = Convert.ToInt32(dr["id_Persona"]),
                                nombre = dr["nombre"].ToString(),
                                apellido = dr["apellido"].ToString()

                            });

                        }

                    }

                }

            }
            catch (Exception)
            {

                lista = new List<deducciones>();
            }




            return lista;
        }



        public int registrar(deducciones obj, out string mensaje)
        {


            int id_Autogenerado = 0;

            mensaje = string.Empty;

            try
            {

                using (SqlConnection con = new SqlConnection(conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarDeduccion", con);

                    cmd.Parameters.AddWithValue("tipo_Deduccion", obj.tipo_Deduccion);
                    cmd.Parameters.AddWithValue("cantidad_Cuotas", obj.cantidad_Cuotas);
                    cmd.Parameters.AddWithValue("fecha_Inicio", obj.fecha_Inicio);
                    cmd.Parameters.AddWithValue("fecha_Finalizacion", obj.fecha_Finalizacion);
                    cmd.Parameters.AddWithValue("monto_Cuota", obj.monto_Cuota);
                    cmd.Parameters.AddWithValue("activo", obj.activo);
                    cmd.Parameters.AddWithValue("id_Persona", obj.id_Persona);
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
        public Boolean editar(deducciones obj, out string mensaje)
        {


            bool resultado = false;

            mensaje = string.Empty;

            try
            {

                using (SqlConnection con = new SqlConnection(conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarDeducciones", con);
                    cmd.Parameters.AddWithValue("id_Deduccion", obj.id_Deduccion);
                    cmd.Parameters.AddWithValue("tipo_Deduccion", obj.tipo_Deduccion);
                    cmd.Parameters.AddWithValue("cantidad_Cuotas", obj.cantidad_Cuotas);
                    cmd.Parameters.AddWithValue("fecha_Inicio", obj.fecha_Inicio);
                    cmd.Parameters.AddWithValue("fecha_Finalizacion", obj.fecha_Finalizacion);
                    cmd.Parameters.AddWithValue("monto_Cuota", obj.monto_Cuota);
                    cmd.Parameters.AddWithValue("activo", obj.activo);
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
                    SqlCommand cmd = new SqlCommand("delete top(1) from deducciones where id_Deduccion =@id", oconexion);
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


    }
}
