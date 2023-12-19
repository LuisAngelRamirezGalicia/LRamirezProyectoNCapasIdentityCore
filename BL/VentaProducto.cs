using Microsoft.Data.SqlClient;
using ML;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class VentaProducto
    {//arreglar metodos de venta prdoducto 

        public static ML.Result Add(ML.VentaProducto ventaProducto)// LINQ 
        {
            Result result = new Result();

            try
            {
                using (DL.LramirezProyectoNcapasIdentityCoreContext context = new DL.LramirezProyectoNcapasIdentityCoreContext())
                {//venta producto 
                    DL.VentaProducto ventadl = new DL.VentaProducto();

                    //ventadl.u= venta.Usuario.UserName.ToString();
                    ventadl.Cantidad = ventaProducto.Cantidad;
                    ventadl.IdProducto = ventaProducto.Producto.IdProducto;
                    

                    //context.Add(ventadl);
                    context.VentaProductos.Add(ventadl);
                    int rowsAffected = context.SaveChanges();

                    //foreach (ML.VentaProducto ventaProductoDos in Objects)
                    //{
                    //    ventaProductoDos.Venta = new ML.Venta();
                    //    ventaProducto.Venta.IdVenta = venta.IdVenta;

                    //    // BL.VentaProducto.AddLINQ(ventaProducto);

                    //    // BL
                    //}

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
        //public static ML.Result AddSP(ML.VentaProducto ventaproducto)
        //{
        //    ML.Result result = new ML.Result();
        //    try
        //    {
        //        using (SqlConnection context = new SqlConnection(DL.Conexion.GetConnectionString()))
        //        {
        //            string query = "VentaProductoAdd";
        //            using (SqlCommand cmd = new SqlCommand())
        //            {
        //                cmd.Connection = context;
        //                cmd.CommandText = query;
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                SqlParameter[] collection = new SqlParameter[3];


        //                collection[0] = new SqlParameter("IdVenta", SqlDbType.Int);
        //                collection[0].Value = ventaproducto.Venta.IdVenta;

        //                collection[1] = new SqlParameter("Cantidad", SqlDbType.Decimal);
        //                collection[1].Value = ventaproducto.Cantidad;

        //                collection[2] = new SqlParameter("IdProducto", SqlDbType.Int);
        //                collection[2].Value = ventaproducto.Producto.IdProducto;

        //                cmd.Parameters.AddRange(collection);

        //                cmd.Connection.Open();

        //                int RowsAffected = cmd.ExecuteNonQuery();

        //                if (RowsAffected > 0)
        //                {
        //                    result.Correct = true;
        //                }
        //                else
        //                {
        //                    result.Correct = false;
        //                    result.ErrorMessage = "Ocurrió un error al insertar el registro en la tabla VentaProducto";
        //                }
        //            }
        //        }


        //    }

        //    catch (Exception ex)
        //    {
        //        result.Correct = false;
        //        result.ErrorMessage = ex.Message;
        //        result.Ex = ex;
        //    }

        //    return result;
        //}
    }
}
