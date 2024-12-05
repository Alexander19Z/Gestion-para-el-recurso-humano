using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_reporteUsuario
    {
        private CD_ReporteUsuario objCapaDato = new CD_ReporteUsuario();

        public List<reporteUsuarios> listar(string departamento)
        {
            return objCapaDato.listar(departamento);
        }


    }
}
