using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_ReporteRegustroVaca
    {

        public List<registro_Vaca> listar(int id_Persona)
        {

            List<registro_Vaca> lista = new List<registro_Vaca>();
            try
            {


                if (id_Persona == 0)
                {

                    //descripcion = descripcion.Replace("-", "/");
                    using (SqlConnection con = new SqlConnection(conexion.cn))
                    {
                        SqlCommand cmd = new SqlCommand(@"select rv.*,p.nombre,p.apellido 
                                                        from registro_Vaca rv
                                                        JOIN persona p ON
                                                         P.id_Persona = rv.id_Persona
                                                        ORDER BY id_Persona ASC", con);


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
                                    fecha_Registro = dr["fecha_Registro"].ToString(),
                                    tipo_Movimiento = dr["tipo_Movimiento"].ToString(),
                                    periodo = dr["periodo"].ToString(),
                                    nombre = dr["nombre"].ToString(),
                                    apellido = dr["apellido"].ToString()

                                });

                            }



                        }



                    }

                }
                else {



                    using (SqlConnection con = new SqlConnection(conexion.cn))
                    {
                        SqlCommand cmd = new SqlCommand(@"select rv.*,p.nombre,p.apellido 
                                                        from registro_Vaca rv
                                                        JOIN persona p ON
                                                         P.id_Persona = rv.id_Persona
                                                        WHERE rv.id_Persona=@id_Persona
                                                        ORDER BY rv.id_Persona ASC", con);

                        cmd.Parameters.AddWithValue("@id_Persona", id_Persona);

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
                                    fecha_Registro = dr["fecha_Registro"].ToString(),
                                    tipo_Movimiento = dr["tipo_Movimiento"].ToString(),
                                    periodo = dr["periodo"].ToString(),
                                    nombre = dr["nombre"].ToString(),
                                    apellido = dr["apellido"].ToString()

                                });

                            }



                        }



                    }



                }

           





            }
            catch (Exception)
            {

                return lista = new List<registro_Vaca>();
            }


            return lista;


        }












    }
}
