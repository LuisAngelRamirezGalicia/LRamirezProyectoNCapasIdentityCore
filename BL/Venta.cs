using ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Venta
    {

        public static ML.Result Add(ML.Venta venta, List<object> Objects)// LINQ 
        {
            Result result = new Result();

            try
            {
                using (DL.LramirezProyectoNcapasIdentityCoreContext context = new DL.LramirezProyectoNcapasIdentityCoreContext())
                {
                    DL.Ventum    ventadl = new DL.Ventum();

                    //ventadl.u= venta.Usuario.UserName.ToString();
                    ventadl.IdCliente = venta.Usuario.IdUsuario;
                    ventadl.Total = venta.Total;
                    ventadl.IdMetodoPago = venta.MetodoPago.IdMetodoPago;

                    //context.Add(ventadl);
                    context.Venta.Add(ventadl);
                    int rowsAffected = context.SaveChanges();

                    foreach (ML.VentaProducto ventaProducto in Objects)
                    {
                        ventaProducto.Venta = new ML.Venta();
                        ventaProducto.Venta.IdVenta = venta.IdVenta;

                       // BL.VentaProducto.AddLINQ(ventaProducto);

                       // BL
                    }

                    if (rowsAffected >= 0)
                    {
                        result.Correct = true;

                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Ocurrio un error al insertar el registro";
                    }
                }
            }

            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }

            return result;

        }
    }

}

