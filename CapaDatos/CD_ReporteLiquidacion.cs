using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_ReporteLiquidacion
    {

        public List<reporteLiquidacion> listar(string descripcion) {

            List<reporteLiquidacion> lista = new List<reporteLiquidacion>();
            try
            {
                //descripcion = descripcion.Replace("-", "/");
                using (SqlConnection con = new SqlConnection(conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand(@"SELECT p.nombre, p.apellido, p.cedula, l.* 
                                                    FROM liquidacion l
                                                    INNER JOIN persona p ON l.id_Persona = p.id_Persona
                                                    WHERE CONVERT(VARCHAR(7), CONVERT(DATETIME, l.descripcion, 103), 120) =@descripcion", con);

                    cmd.Parameters.AddWithValue("@descripcion", descripcion);
                    con.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read()) 
                        {
                            lista.Add(new reporteLiquidacion {
                                nombre = dr["nombre"].ToString(),
                                apellido= dr["apellido"].ToString(),
                                cedula = dr["cedula"].ToString(),
                                id_Liquidacion = Convert.ToInt32(dr["id_Liquidacion"]),
                                descripcion = dr["descripcion"].ToString(),
                                monto_pagar = Convert.ToDecimal(dr["monto_pagar"]),
                                tipo_Liquidacion = dr["tipo_Liquidacion"].ToString(),
                                id_Persona= Convert.ToInt32(dr["id_Persona"]),
                                vacacionesNoDistrutadas = Convert.ToDecimal(dr["vacacionesNoDistrutadas"]),
                                cesantia = Convert.ToDecimal(dr["cesantia"]),
                                aguinaldo = Convert.ToDecimal(dr["aguinaldo"]),
                                preaviso= Convert.ToDecimal(dr["preaviso"])
                            
                            
                            });
                        
                        
                        }

                    }



                }





            }
            catch (Exception)
            {

                return lista = new List<reporteLiquidacion>();
            }


            return lista;


        }

    }
}
