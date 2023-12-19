using System.ComponentModel;
using System.Runtime.Remoting;

//using System.Data.Linq.SqlClient;
namespace BL
{
    public class Producto
    {


        public static ML.Result Add(ML.Producto producto)
        {
            ML.Result result = new ML.Result();
            //aqui se crea el objeto que se retorna

            try
            {   //manda la cadena de conexion 
                using (DL.LramirezProyectoNcapasIdentityCoreContext context = new DL.LramirezProyectoNcapasIdentityCoreContext())

                {   
                    
                    DL.Producto productoLq = new DL.Producto();




                    productoLq.Nombre = producto.Nombre;
                    productoLq.PrecioUnitario = producto.PrecioUnitario;
                    productoLq.Stock = producto.Stock;

                    productoLq.IdProveedor = producto.Proveedor.IdProveedor;
                    productoLq.IdDepartamento = producto.Departamento.IdDepartamento;
                    productoLq.Descripcion = producto.Descripcion;
                    productoLq.Foto = producto.Foto;    
                    

                    context.Productos.Add(productoLq);
                    int RowsAffected = context.SaveChanges();

                    //int query = context.UsuarioAdd(usuario.UserName, usuario.ApellidoPaterno, usuario.ApellidoMaterno, usuario.Email, usuario.Password, usuario.Sexo, usuario.Telefono, usuario.Celular, usuario.FechaNacimiento, usuario.CURP, usuario.Rol.IdRol, usuario.Nombre);

                    //cmd.Parameters.AddWithValue("@ID", usuario.ID);
                    // manda el procedure  y la conexion 
                    if (RowsAffected > 0)
                    {
                        result.Correct = true;
                        result.ErrorMessage = "Materia insertada correctamente";
                    }

                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }


        //update falta modificar a producto 


        public static ML.Result Update(ML.Producto producto)
        {
            ML.Result result = new ML.Result();
            //aqui se crea el objeto que se retorna

            try
            {   //manda la cadena de conexion 
                using (DL.LramirezProyectoNcapasIdentityCoreContext context = new DL.LramirezProyectoNcapasIdentityCoreContext())

                {   /*
                     var materiaLINQ = (from queryLINQ in context.Materias //FROM Materia
                                       where queryLINQ.IdMateria == materia.IdMateria //WHERE IdMateria = 5
                                       select queryLINQ).SingleOrDefault();//SELECT IdMateria,Nombre,Costo,Creditos,IdSemestre

 
                     */

                    var productoLINQ = (from queryLINQ in context.Productos
                                       where queryLINQ.IdProducto == producto.IdProducto
                                       select queryLINQ).SingleOrDefault();

                    if (productoLINQ != null)
                    {
                        //DL_EF.Usuario  = new DL_EF.Usuario();
                        //usuarioLINQ.IdUsuario = usuario.IdUsuario;
                        productoLINQ.Nombre = producto.Nombre;
                       productoLINQ.PrecioUnitario = producto.PrecioUnitario;   
                        productoLINQ.Stock = producto.Stock;
                        productoLINQ.IdProveedor = producto.Proveedor.IdProveedor;
                        productoLINQ.IdDepartamento = producto.Departamento.IdDepartamento;
                        productoLINQ.Descripcion = producto.Descripcion;
                        productoLINQ.Foto = producto.Foto;
                        //context.Usuario.Update(usuarioLINQ);
                        int RowsAffected = context.SaveChanges();

                      
                        //int query = context.UsuarioAdd(usuario.UserName, usuario.ApellidoPaterno, usuario.ApellidoMaterno, usuario.Email, usuario.Password, usuario.Sexo, usuario.Telefono, usuario.Celular, usuario.FechaNacimiento, usuario.CURP, usuario.Rol.IdRol, usuario.Nombre);

                        //cmd.Parameters.AddWithValue("@ID", usuario.ID);
                        // manda el procedure  y la conexion 
                        if (RowsAffected > 0)
                        {
                            result.Correct = true;
                            result.ErrorMessage = "Materia insertada correctamente";
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.LramirezProyectoNcapasIdentityCoreContext context = new DL.LramirezProyectoNcapasIdentityCoreContext())
                {
                    var query = (from producto in context.Productos
                                 select new
                                 {
                                     IdProducto = producto.IdProducto,
                                     Nombre = producto.Nombre,
                                     PrecioUnitario = producto.PrecioUnitario,
                                     Stock = producto.Stock,
                                     IdProovedor = producto.IdProveedor,
                                     IdDepartamento = producto.IdDepartamento,
                                     Descripcion = producto.Descripcion,
                                     Foto = producto.Foto
                                 }).ToList();

                    if (query.Count > 0)
                    {
                        result.Objects = new List<object>();
                        foreach (var productoQuery in query)
                        {
                            ML.Producto producto = new ML.Producto();
                            producto.IdProducto = productoQuery.IdProducto;
                            producto.Nombre = productoQuery.Nombre;
                            producto.PrecioUnitario = productoQuery.PrecioUnitario.Value;
                            producto.Stock = productoQuery.Stock;
                            producto.Descripcion = productoQuery.Descripcion;
                            producto.Departamento = new ML.Departamento();
                            producto.Departamento.IdDepartamento = productoQuery.IdDepartamento.Value;
                            producto.Proveedor = new ML.Proveedor();
                            producto.Proveedor.IdProveedor = productoQuery.IdProovedor.Value;

                            producto.Foto = productoQuery.Foto;


                            result.Objects.Add(producto);

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

        public static ML.Result GetAllBusqueda(string nombre)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.LramirezProyectoNcapasIdentityCoreContext context = new DL.LramirezProyectoNcapasIdentityCoreContext())
                    {
                    var query = (from producto in context.Productos

                                 where producto.Nombre.Contains(nombre)

                                     select new
                                     {
                                         IdProducto = producto.IdProducto,
                                         Nombre = producto.Nombre,
                                         PrecioUnitario = producto.PrecioUnitario,
                                         Stock = producto.Stock,
                                         IdProovedor = producto.IdProveedor,
                                         IdDepartamento = producto.IdDepartamento,
                                         Descripcion = producto.Descripcion,
                                         Foto = producto.Foto
                                     }).ToList();

                    //var query = ML.Producto.Nombre.Where(s => s.Nombre.Contains(nombre)).ToList();

                    if (query.Count > 0)
                    {
                        result.Objects = new List<object>();
                        foreach (var productoQuery in query)
                        {
                            ML.Producto producto = new ML.Producto();
                            producto.IdProducto = productoQuery.IdProducto;
                            producto.Nombre = productoQuery.Nombre;
                            producto.PrecioUnitario = productoQuery.PrecioUnitario.Value;
                            producto.Stock = productoQuery.Stock;
                            producto.Descripcion = productoQuery.Descripcion;
                            producto.Departamento = new ML.Departamento();
                            producto.Departamento.IdDepartamento = productoQuery.IdDepartamento.Value;
                            producto.Proveedor = new ML.Proveedor();
                            producto.Proveedor.IdProveedor = productoQuery.IdProovedor.Value;

                            producto.Foto = productoQuery.Foto;


                            result.Objects.Add(producto);

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

        public static ML.Result ProductoGetByIdDepartamento(int IdDepartamento)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.LramirezProyectoNcapasIdentityCoreContext context = new DL.LramirezProyectoNcapasIdentityCoreContext())
                {
                    var query = (from producto in context.Productos

                                 where producto.IdDepartamento == IdDepartamento

                                 select new
                                 {
                                     IdProducto = producto.IdProducto,
                                     Nombre = producto.Nombre,
                                     PrecioUnitario = producto.PrecioUnitario,
                                     Stock = producto.Stock,
                                     IdProovedor = producto.IdProveedor,
                                     IdDepartamento = producto.IdDepartamento,
                                     Descripcion = producto.Descripcion,
                                     Foto = producto.Foto
                                 }).ToList();

                    //var query = ML.Producto.Nombre.Where(s => s.Nombre.Contains(nombre)).ToList();

                    if (query.Count > 0)
                    {
                        result.Objects = new List<object>();
                        foreach (var productoQuery in query)
                        {
                            ML.Producto producto = new ML.Producto();
                            producto.IdProducto = productoQuery.IdProducto;
                            producto.Nombre = productoQuery.Nombre;
                            producto.PrecioUnitario = productoQuery.PrecioUnitario.Value;
                            producto.Stock = productoQuery.Stock;
                            producto.Descripcion = productoQuery.Descripcion;
                            producto.Departamento = new ML.Departamento();
                            producto.Departamento.IdDepartamento = productoQuery.IdDepartamento.Value;
                            producto.Proveedor = new ML.Proveedor();
                            producto.Proveedor.IdProveedor = productoQuery.IdProovedor.Value;

                            producto.Foto = productoQuery.Foto;


                            result.Objects.Add(producto);

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




        public static ML.Result Delete(int IdProducto)
        {
            ML.Result result = new ML.Result();
            //aqui se crea el objeto que se retorna

            try
            {   //manda la cadena de conexion 
                using (DL.LramirezProyectoNcapasIdentityCoreContext context = new DL.LramirezProyectoNcapasIdentityCoreContext())

                {
                    var ProductoLINQ = (from queryLINQ in context.Productos
                                       where queryLINQ.IdProducto == IdProducto
                                       select queryLINQ).SingleOrDefault();

                    if (ProductoLINQ != null)
                    {

                        
                        context.Productos.Remove(ProductoLINQ);
                        //context.Usuario.Update(usuarioLINQ);
                        int RowsAffected = context.SaveChanges();

                        //int query = context.UsuarioAdd(usuario.UserName, usuario.ApellidoPaterno, usuario.ApellidoMaterno, usuario.Email, usuario.Password, usuario.Sexo, usuario.Telefono, usuario.Celular, usuario.FechaNacimiento, usuario.CURP, usuario.Rol.IdRol, usuario.Nombre);

                        //cmd.Parameters.AddWithValue("@ID", usuario.ID);
                        // manda el procedure  y la conexion 
                        if (RowsAffected > 0)
                        {
                            result.Correct = true;
                            result.ErrorMessage = "Producto borrado correctamente";
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }


        /// get by id
        /// 
        public static ML.Result GetById(int IdProducto)

        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.LramirezProyectoNcapasIdentityCoreContext context = new DL.LramirezProyectoNcapasIdentityCoreContext())
                {
                    var productoQuery = (from ProductoLINQ in context.Productos //FROM Materia
                                                                        //join rolLINQ in context.Rol on usuarioLINQ.IdRol equals rolLINQ.IdRol
                                 where ProductoLINQ.IdProducto == IdProducto
                                 //  join aliasVaraiable in tablaModeloB on tablaModeloA.FK equals tablaModeloB.PK
                                 select new
                                 {
                                     Id = ProductoLINQ.IdProducto,
                                     Nombre = ProductoLINQ.Nombre,
                                     PrecioUnitario = ProductoLINQ.PrecioUnitario,
                                     Stock = ProductoLINQ.Stock,
                                     IdProovedor = ProductoLINQ.IdProveedor,
                                     IdDepartamento = ProductoLINQ.IdDepartamento,
                                     Descripcion = ProductoLINQ.Descripcion


                                 }).SingleOrDefault();

                    /// EL Single Or Defaut sirve para aclarar que se retornna un solo objeto 
                    //  result.Objects = new List<object>();

                    if (productoQuery != null)
                    {
                        // foreach (var obj in query)
                        // {

                        ML.Producto producto = new ML.Producto();
                        producto.IdProducto = productoQuery.Id;
                        producto.Nombre = productoQuery.Nombre;
                        producto.PrecioUnitario = productoQuery.PrecioUnitario.Value;
                        producto.Stock = productoQuery.Stock;
                        producto.Descripcion = productoQuery.Descripcion;
                        producto.Departamento = new ML.Departamento();
                        producto.Departamento.IdDepartamento = productoQuery.IdDepartamento.Value;
                        producto.Proveedor = new ML.Proveedor();
                        producto.Proveedor.IdProveedor = productoQuery.IdProovedor.Value;

                        result.Object = producto;




                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se encontraron registros";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }
    }
}