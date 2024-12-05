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
   public class CD_Notificacion
    {

        public List<notificaciones> listar()
        {



            List<notificaciones> lista = new List<notificaciones>();


            try
            {

                using (SqlConnection con = new SqlConnection(conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand(@"SELECT n.*,p.nombre as nombre, p.apellido as apellido 
                                                      FROM notificaciones n
                                                      JOIN persona p ON n.id_Persona = p.id_Persona", con);
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new notificaciones
                            {
                                id_Noti = Convert.ToInt32(dr["id_Noti"]),
                                tipo_Notificacion =(dr["tipo_Notificacion"]).ToString(),
                                estado = Convert.ToBoolean(dr["estado"]),
                                id_Persona = Convert.ToInt32(dr["id_Persona"]),
                                fechasSolicitadas= (dr["fechasSolicitadas"]).ToString(),
                                dias_Solicitado= Convert.ToDecimal(dr["dias_Solicitado"]),
                                horas_solicitadas = Convert.ToDecimal(dr["horas_solicitadas"]),
                                aprobacionRRHH = Convert.ToBoolean(dr["aprobacionRRHH"]),
                                nombre = (dr["nombre"]).ToString(),
                                apellido = (dr["apellido"]).ToString()


                            }); 

                        }

                    }

                }

            }
            catch (Exception)
            {

                lista = new List<notificaciones>();
            }




            return lista;
        }

        public int registrar(notificaciones obj, out string mensaje)
        {


            int id_Autogenerado = 0;

            mensaje = string.Empty;

            try
            {

                using (SqlConnection con = new SqlConnection(conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarNotificaciones", con);
                    cmd.Parameters.AddWithValue("tipo_Notificacion", obj.tipo_Notificacion);
                    cmd.Parameters.AddWithValue("estado", obj.estado);
                    cmd.Parameters.AddWithValue("id_Persona", obj.id_Persona);
                    cmd.Parameters.AddWithValue("fechasSolicitadas", obj.fechasSolicitadas);
                    cmd.Parameters.AddWithValue("dias_Solicitado", obj.dias_Solicitado);
                    cmd.Parameters.AddWithValue("horas_solicitadas", obj.horas_solicitadas);
                    cmd.Parameters.AddWithValue("aprobacionRRHH", obj.aprobacionRRHH);

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



        public Boolean editar(notificaciones obj, out string mensaje)
        {


            bool resultado = false;

            mensaje = string.Empty;

            try
            {

                using (SqlConnection con = new SqlConnection(conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarNotificaciones", con);
                    cmd.Parameters.AddWithValue("id_Noti", obj.id_Noti);
                    cmd.Parameters.AddWithValue("tipo_Notificacion", obj.tipo_Notificacion);
                    cmd.Parameters.AddWithValue("estado", obj.estado);
                    cmd.Parameters.AddWithValue("id_Persona", obj.id_Persona);
                    cmd.Parameters.AddWithValue("fechasSolicitadas", obj.fechasSolicitadas);
                    cmd.Parameters.AddWithValue("dias_Solicitado", obj.dias_Solicitado);
                    cmd.Parameters.AddWithValue("horas_solicitadas", obj.horas_solicitadas);
                    cmd.Parameters.AddWithValue("aprobacionRRHH", obj.aprobacionRRHH);
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
                    SqlCommand cmd = new SqlCommand("delete top(1) from notificaciones where id_Noti =@id", oconexion);
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





        public string BuscarLider(int idDepartamento, out string mensaje)
        {
            
            string correo = "";
            mensaje = string.Empty;

            try
            {
                using (SqlConnection con = new SqlConnection(conexion.cn))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_buscarLider", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id_departamento", idDepartamento);

                        con.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                correo = reader["correo_Empresarial"].ToString();  
                             
                            }
                        }

                        if (correo == null)
                        {
                            mensaje = "No se encontraron resultados.";
                        }
                        else
                        {
                            mensaje = "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }

            return correo;
        }




        public List<listarSolicitudes> listarSolicitudes(int idDepartamento)
        {



            List<listarSolicitudes> lista = new List<listarSolicitudes>();


            try
            {

                using (SqlConnection con = new SqlConnection(conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand(@"SELECT n.id_Noti,n.tipo_Notificacion,n.aprobacionRRHH,n.estado,p.id_Persona,p.nombre,p.apellido,n.fechasSolicitadas,n.dias_Solicitado,n.horas_solicitadas,d.descripcion 
                                                      FROM notificaciones AS n 
                                                      INNER JOIN persona AS p ON n.id_Persona = p.id_Persona 
                                                      INNER JOIN departamento AS d ON p.id_Departamento = d.id_Departamento 
                                                      WHERE n.estado = 0 AND p.id_Departamento = @id_Departamento;", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@id_Departamento", idDepartamento);
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new listarSolicitudes
                            {
                                id_Noti = Convert.ToInt32(dr["id_Noti"]),
                                tipo_Notificacion = (dr["tipo_Notificacion"]).ToString(),
                                estado = Convert.ToBoolean(dr["estado"]),
                                id_Persona= Convert.ToInt32(dr["id_Persona"]),
                                nombre = (dr["nombre"]).ToString(),
                                apellido= (dr["apellido"]).ToString(),
                                fechasSolicitadas = (dr["fechasSolicitadas"]).ToString(),
                                dias_Solicitado = Convert.ToDecimal(dr["dias_Solicitado"]),
                                horas_solicitadas = Convert.ToDecimal(dr["horas_solicitadas"]),
                                aprobacionRRHH = Convert.ToBoolean(dr["aprobacionRRHH"]),
                                descripcion = (dr["descripcion"]).ToString()

                            }); ;

                        }

                    }

                }

            }
            catch (Exception)
            {

                lista = new List<listarSolicitudes>();
            }




            return lista;
        }



        public List<listarSolicitudes> listarSolicitudesRRHH()
        {



            List<listarSolicitudes> lista = new List<listarSolicitudes>();


            try
            {

                using (SqlConnection con = new SqlConnection(conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand(@"SELECT n.id_Noti,n.tipo_Notificacion,n.aprobacionRRHH,n.estado,p.id_Persona,p.nombre,p.apellido,n.fechasSolicitadas,n.dias_Solicitado,n.horas_solicitadas,d.descripcion 
                                                      FROM notificaciones AS n 
                                                      INNER JOIN persona AS p ON n.id_Persona = p.id_Persona 
                                                      INNER JOIN departamento AS d ON p.id_Departamento = d.id_Departamento 
                                                      WHERE n.estado = 1 AND n.aprobacionRRHH=0", con);
                    cmd.CommandType = CommandType.Text;
                    
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new listarSolicitudes
                            {
                                id_Noti = Convert.ToInt32(dr["id_Noti"]),
                                tipo_Notificacion = (dr["tipo_Notificacion"]).ToString(),
                                estado = Convert.ToBoolean(dr["estado"]),
                                id_Persona = Convert.ToInt32(dr["id_Persona"]),
                                nombre = (dr["nombre"]).ToString(),
                                apellido = (dr["apellido"]).ToString(),
                                fechasSolicitadas = (dr["fechasSolicitadas"]).ToString(),
                                dias_Solicitado = Convert.ToDecimal(dr["dias_Solicitado"]),
                                horas_solicitadas = Convert.ToDecimal(dr["horas_solicitadas"]),
                                aprobacionRRHH = Convert.ToBoolean(dr["aprobacionRRHH"]),
                                descripcion = (dr["descripcion"]).ToString()

                            }); ;

                        }

                    }

                }

            }
            catch (Exception)
            {

                lista = new List<listarSolicitudes>();
            }




            return lista;
        }




        public List<listarSolicitudes> listarSolicitudesGerente()
        {



            List<listarSolicitudes> lista = new List<listarSolicitudes>();


            try
            {

                using (SqlConnection con = new SqlConnection(conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand(@"SELECT n.id_Noti,n.tipo_Notificacion,n.aprobacionRRHH,n.estado,p.id_Persona,p.nombre,p.apellido,n.fechasSolicitadas,n.dias_Solicitado,n.horas_solicitadas,d.descripcion 
                                                      FROM notificaciones AS n 
                                                      INNER JOIN persona AS p ON n.id_Persona = p.id_Persona 
                                                      INNER JOIN puesto AS d ON p.id_Puesto = d.id_Puesto 
                                                      WHERE d.lider=1 and n.aprobacionRRHH =0;", con);
                    cmd.CommandType = CommandType.Text;

                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new listarSolicitudes
                            {
                                id_Noti = Convert.ToInt32(dr["id_Noti"]),
                                tipo_Notificacion = (dr["tipo_Notificacion"]).ToString(),
                                estado = Convert.ToBoolean(dr["estado"]),
                                id_Persona = Convert.ToInt32(dr["id_Persona"]),
                                nombre = (dr["nombre"]).ToString(),
                                apellido = (dr["apellido"]).ToString(),
                                fechasSolicitadas = (dr["fechasSolicitadas"]).ToString(),
                                dias_Solicitado = Convert.ToDecimal(dr["dias_Solicitado"]),
                                horas_solicitadas = Convert.ToDecimal(dr["horas_solicitadas"]),
                                aprobacionRRHH = Convert.ToBoolean(dr["aprobacionRRHH"]),
                                descripcion = (dr["descripcion"]).ToString()

                            }); ;

                        }

                    }

                }

            }
            catch (Exception)
            {

                lista = new List<listarSolicitudes>();
            }




            return lista;
        }






        public bool actualizarEstado(int id_Noti, int estado, int aprobacionRRHH)
        {

            bool resultado = false;


            try
            {

                using (SqlConnection con = new SqlConnection(conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("UPDATE notificaciones set estado=@estado,aprobacionRRHH = @aprobacionRRHH where id_Noti = @id_Noti", con);
                    cmd.Parameters.AddWithValue("@id_Noti", id_Noti);
                    cmd.Parameters.AddWithValue("@estado", estado);
                    cmd.Parameters.AddWithValue("@aprobacionRRHH", aprobacionRRHH);
                    cmd.CommandType = CommandType.Text;


                    con.Open();
                    cmd.ExecuteNonQuery();
                    

                    resultado=true;

                }

            }
            catch (Exception ex)
            {

                resultado = false;
               
            }

            return resultado;
        }





        public bool aprobarPersimo(int id_Persona)
        {

            bool resultado = false;


            try
            {

                using (SqlConnection con = new SqlConnection(conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("UPDATE permiso set aprobado=1where id_Persona = @id_Persona", con);
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

        public List<listarSolicitudes> listarSolicitudesEmpleado()
        {



            List<listarSolicitudes> lista = new List<listarSolicitudes>();


            try
            {

                using (SqlConnection con = new SqlConnection(conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand(@"SELECT n.id_Noti,n.tipo_Notificacion,n.aprobacionRRHH,n.estado,p.id_Persona,p.nombre,p.apellido,n.fechasSolicitadas,n.dias_Solicitado,n.horas_solicitadas,d.descripcion 
                                                      FROM notificaciones AS n 
                                                      INNER JOIN persona AS p ON n.id_Persona = p.id_Persona 
                                                      INNER JOIN departamento AS d ON p.id_Departamento = d.id_Departamento 
                                                      WHERE n.estado = 1 AND n.aprobacionRRHH=1", con);
                    cmd.CommandType = CommandType.Text;

                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new listarSolicitudes
                            {
                                id_Noti = Convert.ToInt32(dr["id_Noti"]),
                                tipo_Notificacion = (dr["tipo_Notificacion"]).ToString(),
                                estado = Convert.ToBoolean(dr["estado"]),
                                id_Persona = Convert.ToInt32(dr["id_Persona"]),
                                nombre = (dr["nombre"]).ToString(),
                                apellido = (dr["apellido"]).ToString(),
                                fechasSolicitadas = (dr["fechasSolicitadas"]).ToString(),
                                dias_Solicitado = Convert.ToDecimal(dr["dias_Solicitado"]),
                                horas_solicitadas = Convert.ToDecimal(dr["horas_solicitadas"]),
                                aprobacionRRHH = Convert.ToBoolean(dr["aprobacionRRHH"]),
                                descripcion = (dr["descripcion"]).ToString()

                            }); ;

                        }

                    }

                }

            }
            catch (Exception)
            {

                lista = new List<listarSolicitudes>();
            }




            return lista;
        }






    }
}
