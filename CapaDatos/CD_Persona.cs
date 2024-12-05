using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace CapaDatos
{
    public class CD_Persona
    {

        public List<persona> listar() {



            List<persona> lista = new List<persona>();


            try
            {

                using (SqlConnection con = new SqlConnection(conexion.cn))
                {


                        string query = @"
                    SELECT p.*, d.descripcion AS DepartamentoDescripcion, pu.descripcion AS PuestoDescripcion
                    FROM persona p
                    JOIN departamento d ON p.id_Departamento = d.id_Departamento
                    JOIN puesto pu ON p.id_Puesto = pu.id_Puesto";


                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read()) {
                            lista.Add(new persona
                            {
                                id_Persona = Convert.ToInt32(dr["id_Persona"]),
                                nombre = dr["nombre"].ToString(),
                                apellido = dr["apellido"].ToString(),
                                cedula = dr["cedula"].ToString(),
                                sexo = dr["sexo"].ToString(),
                                usuario = dr["usuario"].ToString(),
                                contrasena = dr["contrasena"].ToString(),
                                correo_Empresarial = dr["correo_Empresarial"].ToString(),
                                activo = Convert.ToBoolean(dr["activo"]),
                                fecha_ingreso = Convert.ToDateTime(dr["fecha_ingreso"]),
                                fecha_salida = Convert.ToDateTime(dr["fecha_salida"]),
                                tipo_Usuario = dr["tipo_Usuario"].ToString(),
                                id_Departamento = Convert.ToInt32(dr["id_Departamento"]),
                                id_Puesto = Convert.ToInt32(dr["id_Puesto"]),
                                departamentoDescripcion = dr["DepartamentoDescripcion"].ToString(),
                                puestoDescripcion = dr["puestoDescripcion"].ToString(),
                                cantidad_Hijos = Convert.ToInt32(dr["cantidad_Hijos"]),
                                estado_Civil = dr["estado_Civil"].ToString(),
                                 telefono = dr["telefono"].ToString(),
                                direccion = dr["direccion"].ToString(),
                                canton = dr["canton"].ToString(),
                                distrito = dr["distrito"].ToString(),
                                provincia = dr["provincia"].ToString(),


                            }); ; 
                        
                        }

                    }

                }

            }
            catch (Exception)
            {

                lista = new List<persona>();
            }




            return lista;
        }  
        

        public int registrar(persona obj,out string mensaje)
        {


            int id_Autogenerado = 0;

            mensaje = string.Empty;

            try
            {

                using (SqlConnection con = new SqlConnection(conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarPersona",con);
                    cmd.Parameters.AddWithValue("nombre",obj.nombre);
                    cmd.Parameters.AddWithValue("apellido", obj.apellido+" "+obj.apellidoS);
                    cmd.Parameters.AddWithValue("cedula", obj.cedula);
                    cmd.Parameters.AddWithValue("sexo", obj.sexo);
                    cmd.Parameters.AddWithValue("usuario", obj.usuario);
                    cmd.Parameters.AddWithValue("contrasena", obj.contrasena);
                    cmd.Parameters.AddWithValue("correo_Empresarial", obj.correo_Empresarial);
                    cmd.Parameters.AddWithValue("activo", obj.activo);
                    cmd.Parameters.AddWithValue("fecha_ingreso", obj.fecha_ingreso);
                    cmd.Parameters.AddWithValue("fecha_salida", obj.fecha_salida);
                    cmd.Parameters.AddWithValue("tipo_Usuario", obj.tipo_Usuario);
                    cmd.Parameters.AddWithValue("cantidad_Hijos", obj.cantidad_Hijos);
                    cmd.Parameters.AddWithValue("estado_Civil", obj.estado_Civil);
                    cmd.Parameters.AddWithValue("id_Departamento", obj.id_Departamento);
                    cmd.Parameters.AddWithValue("id_Puesto", obj.id_Puesto);
                    cmd.Parameters.AddWithValue("telefono", obj.telefono);
                    cmd.Parameters.AddWithValue("direccion", obj.distrito + " " + obj.canton + " " + obj.provincia);
                    cmd.Parameters.AddWithValue("canton", obj.canton);
                    cmd.Parameters.AddWithValue("distrito", obj.distrito);
                    cmd.Parameters.AddWithValue("provincia", obj.provincia);
                    cmd.Parameters.Add("Resultado",SqlDbType.Int).Direction=ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar,500).Direction=ParameterDirection.Output;
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






        public Boolean editar(persona obj, out string mensaje)
        {


            bool resultado = false;

            mensaje = string.Empty;

            try
            {

                using (SqlConnection con = new SqlConnection(conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarPersona", con);
                    cmd.Parameters.AddWithValue("id_Persona", obj.id_Persona);
                    cmd.Parameters.AddWithValue("nombre", obj.nombre);
                    cmd.Parameters.AddWithValue("apellido", obj.apellido + " " + obj.apellidoS);
                    cmd.Parameters.AddWithValue("cedula", obj.cedula);
                    cmd.Parameters.AddWithValue("sexo", obj.sexo);
                    cmd.Parameters.AddWithValue("usuario", obj.usuario);
                    cmd.Parameters.AddWithValue("correo_Empresarial", obj.correo_Empresarial);
                    cmd.Parameters.AddWithValue("activo", obj.activo);
                    cmd.Parameters.AddWithValue("fecha_ingreso", obj.fecha_ingreso);
                    cmd.Parameters.AddWithValue("fecha_salida", obj.fecha_salida);
                    cmd.Parameters.AddWithValue("tipo_Usuario", obj.tipo_Usuario);
                    cmd.Parameters.AddWithValue("id_Departamento", obj.id_Departamento);
                    cmd.Parameters.AddWithValue("id_Puesto", obj.id_Puesto);
                    cmd.Parameters.AddWithValue("cantidad_Hijos", obj.cantidad_Hijos);
                    cmd.Parameters.AddWithValue("estado_Civil", obj.estado_Civil);
                    cmd.Parameters.AddWithValue("telefono", obj.telefono);
                    cmd.Parameters.AddWithValue("direccion", obj.distrito+", "+obj.canton+", "+obj.provincia);
                    cmd.Parameters.AddWithValue("canton", obj.canton);
                    cmd.Parameters.AddWithValue("distrito", obj.distrito);
                    cmd.Parameters.AddWithValue("provincia", obj.provincia);
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
                    SqlCommand cmd = new SqlCommand("delete top(1) from persona where id_Persona =@id", oconexion);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }


            }
            catch (Exception ex)
            {

                resultado = false;
                mensaje = "No se puede eliminar el registro porque tiene datos relacionados. " + ex.Message;
            }
            return resultado;
        }


        public Boolean restablecerCorreo(int id,string correoEmpresarial,string contrasena, out string mensaje)
        {


            bool resultado = false;

            mensaje = string.Empty;

            try
            {

                using (SqlConnection con = new SqlConnection(conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RestablecerCorreo", con);
                    cmd.Parameters.AddWithValue("id_Persona",id);
                    cmd.Parameters.AddWithValue("correo_Empresarial",correoEmpresarial);
                    cmd.Parameters.AddWithValue("contrasena", contrasena);

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

        public bool cambiarClave(int id,string nuevaClave, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;
            try
            {

                using (SqlConnection oconexion = new SqlConnection(conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("UPDATE persona set contrasena=@nuevaClave WHERE id_Persona=@id", oconexion);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@nuevaClave", nuevaClave);
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
