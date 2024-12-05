using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Aguinaldo
    {
        private CD_Aguinaldo objCapaDato = new CD_Aguinaldo();

        public List<aguinaldo> listar()
        {
            return objCapaDato.listar();
        }


        public int registrar(aguinaldo obj, out string mensaje)
        {
            string res = validaciones(obj);
            if (string.IsNullOrEmpty(res))
            {
                return objCapaDato.registrar(obj, out mensaje);
            }
            else { 
                mensaje = res;
                return 0;
            
            }

      


        }

        public Boolean editar(aguinaldo obj, out string mensaje)
        {
            string res = validaciones(obj);
            if (string.IsNullOrEmpty(res))
            {
                return objCapaDato.editar(obj, out mensaje);
            }
            else
            {
                mensaje = res;
                return false;

            }


            


        }

        public bool eliminar(int id, out string mensaje)
        {
            return objCapaDato.eliminar(id, out mensaje);
        }

        public List<aguinaldoMeses> listarMeses()
        {
            try
            {
          
                var aguinaldoCalular = objCapaDato.listarMeses();
                var aguinaldoFinal = new List<aguinaldoMeses>();
                DateTime anioActual = DateTime.Now;
            
                var respuesta = 0;
                string mensaje = string.Empty;
                objCapaDato.eliminarAguinaldoCalulado();

                foreach (var aguinaldo in aguinaldoCalular)
                {
                    aguinaldoMeses nuevo = new aguinaldoMeses {
                        id_Persona = aguinaldo.id_Persona,
                        Diciembre_Anterior = aguinaldo.Diciembre_Anterior,
                        Enero = aguinaldo.Enero,
                        Febrero = aguinaldo.Febrero,
                        Marzo = aguinaldo.Marzo,
                        Abril = aguinaldo.Abril,
                        Mayo = aguinaldo.Mayo,
                        Junio = aguinaldo.Junio,
                        Julio = aguinaldo.Julio,
                        Agosto = aguinaldo.Agosto,
                        Septiembre = aguinaldo.Septiembre,
                        Octubre = aguinaldo.Octubre,
                        Noviembre = aguinaldo.Noviembre,
                        nombre=aguinaldo.nombre,
                        apellido=aguinaldo.apellido,

                        TotalAnio = aguinaldo.TotalAnio,
                        aguinaldoPagar = Math.Round(aguinaldo.TotalAnio / 12,2)
                      

                
                    };
                    aguinaldoFinal.Add(nuevo);

                    aguinaldo objAguinaldo = new aguinaldo { 
                    
                        monto_Total=nuevo.aguinaldoPagar,
                        fecha_Pago= anioActual,
                        id_Persona=nuevo.id_Persona



                    };

                    respuesta= objCapaDato.registrar(objAguinaldo,out mensaje);

                }

                    return aguinaldoFinal;

            }
            catch (Exception)
            {

                throw;
            }


            

        }




        public string validaciones(aguinaldo obj) {
            string mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.monto_Total.ToString()) || string.IsNullOrWhiteSpace(obj.monto_Total.ToString()))
            {
                mensaje = "El campo monto total no puede estar en blanco.";
            }

            if (string.IsNullOrEmpty(obj.fecha_Pago.ToString()) || string.IsNullOrWhiteSpace(obj.fecha_Pago.ToString()))
            {
                mensaje = "El campo fecha de pago no puede estar vacío.";
            }


            if (string.IsNullOrEmpty(obj.id_Persona.ToString()) || string.IsNullOrWhiteSpace(obj.id_Persona.ToString()))
            {
                mensaje = "El campo persona de pago no puede estar vacío.";
            }





            return mensaje;
        }













    }
}
