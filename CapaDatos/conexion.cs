using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace CapaDatos
{
    public class conexion
    {
        public static string cn = ConfigurationManager.ConnectionStrings["cadena"].ToString();
    }
}
