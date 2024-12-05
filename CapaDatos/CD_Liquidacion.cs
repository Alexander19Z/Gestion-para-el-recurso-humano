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
    public class CD_Liquidacion
    {

        public List<liquidacion> listar()
        {



            List<liquidacion> lista = new List<liquidacion>();


            try
            {

                using (SqlConnection con = new SqlConnection(conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand(@"SELECT n.*,p.nombre as nombre, p.apellido as apellido 
                                                      FROM liquidacion n
                                                      JOIN persona p ON n.id_Persona = p.id_Persona", con);
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new liquidacion
                            {
                                id_Liquidacion = Convert.ToInt32(dr["id_Liquidacion"]),
                                descripcion = dr["descripcion"].ToString(),
                                monto_pagar = Convert.ToDecimal(dr["monto_pagar"]),
                                tipo_Liquidacion = dr["tipo_Liquidacion"].ToString(),
                                id_Persona = Convert.ToInt32(dr["id_persona"]),
                                vacacionesNoDistrutadas= Convert.ToDecimal(dr["vacacionesNoDistrutadas"]),
                                aguinaldo = Convert.ToDecimal(dr["aguinaldo"]),
                                preaviso = Convert.ToDecimal(dr["preaviso"]),
                                cesantia = Convert.ToDecimal(dr["cesantia"]),
                                 nombre = (dr["nombre"]).ToString(),
                                apellido = (dr["apellido"]).ToString(),
                                diasTrabajados = Convert.ToDecimal(dr["diasTrabajados"])


                            }); ;

                        }

                    }

                }

            }
            catch (Exception)
            {

                lista = new List<liquidacion>();
            }




            return lista;
        }

        public int registrar(liquidacion obj, out string mensaje)
        {


            int id_Autogenerado = 0;

            mensaje = string.Empty;

            try
            {

                using (SqlConnection con = new SqlConnection(conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarLiquidacion", con);
                    cmd.Parameters.AddWithValue("descripcion", obj.descripcion);
                    cmd.Parameters.AddWithValue("monto_pagar", obj.monto_pagar);
                    cmd.Parameters.AddWithValue("tipo_Liquidacion", obj.tipo_Liquidacion);
                    cmd.Parameters.AddWithValue("id_Persona", obj.id_Persona);
                    cmd.Parameters.AddWithValue("vacacionesNoDistrutadas", obj.vacacionesNoDistrutadas);
                    cmd.Parameters.AddWithValue("aguinaldo", obj.aguinaldo);
                    cmd.Parameters.AddWithValue("preaviso", obj.preaviso);
                    cmd.Parameters.AddWithValue("cesantia", obj.cesantia);
                    cmd.Parameters.AddWithValue("diasTrabajados", obj.diasTrabajados);
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



        public Boolean editar(liquidacion obj, out string mensaje)
        {


            bool resultado = false;

            mensaje = string.Empty;

            try
            {

                using (SqlConnection con = new SqlConnection(conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarLiquidacion", con);
                    cmd.Parameters.AddWithValue("id_Liquidacion", obj.id_Liquidacion);
                    cmd.Parameters.AddWithValue("descripcion", obj.descripcion);
                    cmd.Parameters.AddWithValue("monto_pagar", obj.monto_pagar);
                    cmd.Parameters.AddWithValue("tipo_Liquidacion", obj.tipo_Liquidacion);
                    cmd.Parameters.AddWithValue("id_Persona", obj.id_Persona);
                    cmd.Parameters.AddWithValue("vacacionesNoDistrutadas", obj.vacacionesNoDistrutadas);
                    cmd.Parameters.AddWithValue("aguinaldo", obj.aguinaldo);
                    cmd.Parameters.AddWithValue("preaviso", obj.preaviso);
                    cmd.Parameters.AddWithValue("cesantia", obj.cesantia);
                    cmd.Parameters.AddWithValue("diasTrabajados", obj.diasTrabajados);
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
                    SqlCommand cmd = new SqlCommand("delete top(1) from liquidacion where id_liquidacion =@id", oconexion);
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



        public List<PersonaLiquidacion> buscarPersona(int id, out string mensaje)
        {
            
            mensaje = string.Empty;
            List<PersonaLiquidacion> lista = new List<PersonaLiquidacion>();
            try
            {

                using (SqlConnection con = new SqlConnection(conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand(@"SELECT p.id_Persona, p.nombre,p.apellido,p.cedula,p.fecha_ingreso,pu.salario_Bruto,v.saldo_Vacaciones from persona p
                                                      join puesto pu ON p.id_Puesto =pu.id_Puesto
                                                      join vacaciones v ON p.id_Persona = v.id_Persona 
                                                      where p.cedula=@id and p.activo= 1" , con);
                    cmd.Parameters.AddWithValue("@id", id.ToString().Trim());
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new PersonaLiquidacion
                            {
                                id_Persona = Convert.ToInt32(dr["id_Persona"]),
                                nombre = (dr["nombre"]).ToString(),
                                apellido = (dr["apellido"]).ToString(),
                                cedula = (dr["cedula"]).ToString(),
                                fecha_ingreso= (dr["fecha_ingreso"]).ToString(),
                                salario_Bruto = Convert.ToDecimal(dr["salario_Bruto"]),
                                saldo_Vacaciones= Convert.ToDecimal(dr["saldo_Vacaciones"])



                            }); ;

                        }

                    }
                }

                return lista;
            }
            catch (Exception ex)
            {

             
                mensaje = ex.Message;
                return lista = new List<PersonaLiquidacion>();
            }
            

        }

        public bool actualizarPersonaSalida(DateTime fechaSalida,int id_Persona) {


            try
            {

                using (SqlConnection con = new SqlConnection(conexion.cn))
                {

                    using (SqlCommand cmd = new SqlCommand(@"UPDATE persona set 
                                                            activo=0,
                                                            fecha_salida=@fecha_salida 
                                                            WHERE id_Persona=@id_Persona",con))
                    {
                        cmd.Parameters.AddWithValue("@fecha_salida ", fechaSalida);
                        cmd.Parameters.AddWithValue("@id_Persona", id_Persona);
                        con.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();


                        if (rowsAffected > 0)
                        {
                            return true;
                        }
                        else
                        {
                          return false;
                        }

                    }

                }


            }
            catch (Exception)
            {

                return false;
            }













          

        }



    }
}
