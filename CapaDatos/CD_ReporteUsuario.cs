using CapaEntidad;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_ReporteUsuario
    {

        public List<reporteUsuarios> listar(string departamento)
        {
            List<reporteUsuarios> lista = new List<reporteUsuarios>();
            try
            {
                using (SqlConnection con = new SqlConnection(conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand(@"
                                                    SELECT p.nombre, p.apellido, d.descripcion AS departamento, pu.descripcion AS puesto 
                                                    FROM persona p
                                                    INNER JOIN departamento d ON d.id_Departamento = p.id_Departamento
                                                    INNER JOIN puesto pu ON pu.id_Puesto = p.id_Puesto
                                                    WHERE d.descripcion = @departamento", con);

                    cmd.Parameters.AddWithValue("@departamento", departamento);
                    con.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new reporteUsuarios
                            {
                                nombre = dr["nombre"].ToString(),
                                apellido = dr["apellido"].ToString(),
                                departamento = dr["departamento"].ToString(), // corregido aquí
                                puesto = dr["puesto"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception)
            {
                
                return lista = new List<reporteUsuarios>();
            }

            return lista;
        }



    }
}
