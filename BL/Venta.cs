using DL;
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
        public static ML.Result GetByUsuario(string nombre)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.LramirezProyectoNcapasIdentityCoreContext context = new DL.LramirezProyectoNcapasIdentityCoreContext())
                {
                    var query = (from venta in context.Venta
                                     //inner join AspNetUsers on venta.IdCliente = AspNetUsers.Id
                                     //inner join MetodoPago on  Venta.IdMetodoPago = MetodoPago.IdMetodoPago
                                     // join rolLINQ in context.Rol on usuarioLINQ.IdRol equals rolLINQ.IdRol
                                     //join cat in cats on person equals cat.Owner
                                     // join aspNetUser  in on
                                     // 
                                     join usuario in context.AspNetUsers on venta.IdCliente equals usuario.Id
                                     join metodoPago in context.MetodoPagos on venta.IdMetodoPago equals metodoPago.IdMetodoPago

                                 where venta.IdCliente.Contains(nombre)

                                 select new
                                 {
                                     Idventa = venta.IdVenta,
                                     IdCliente = venta.IdCliente,
                                     Total = venta.Total,
                                     IdMetodoPago = venta.IdMetodoPago,
                                     Fecha = venta.Fecha,
                                     Nombre = usuario.UserName,
                                     pago = metodoPago.Metodo

                                 }).ToList();

                    //var query = ML.Producto.Nombre.Where(s => s.Nombre.Contains(nombre)).ToList();

                    if (query.Count > 0)
                    {
                        result.Objects = new List<object>();
                        foreach (var productoQuery in query)
                        {
                            ML.Venta venta = new ML.Venta();

                            venta.IdVenta = productoQuery.Idventa;
                            venta.Usuario = new ML.Usuario();
                            venta.Usuario.IdUsuario = productoQuery.IdCliente;
                            venta.Total = productoQuery.Total;
                            venta.MetodoPago = new ML.MetodoPago();
                            venta.MetodoPago.IdMetodoPago = productoQuery.IdMetodoPago.Value;
                            venta.Usuario.Correo = productoQuery.Nombre;
                            venta.MetodoPago.Nombre = productoQuery.pago;

                            result.Objects.Add(venta);

                        }
                        result.Correct = true;
                    }
                }
            }
            catch (Exception e)
            {
                result.Correct = false;
                result.ErrorMessage = e.InnerException.Message;
                result.Ex = e;
            }

            return result;
        }


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

