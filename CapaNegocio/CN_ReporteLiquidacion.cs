using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace CapaNegocio
{
    public class CN_ReporteLiquidacion
    {
        private CD_ReporteLiquidacion objCapaDato = new CD_ReporteLiquidacion();

        public List<reporteLiquidacion> listar(string descripcion)
        {
            return objCapaDato.listar(descripcion); 
        }




    }
}
