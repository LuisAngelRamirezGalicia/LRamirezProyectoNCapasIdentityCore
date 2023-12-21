using BL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Abstractions;
using System.Collections.Specialized;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace PL.Controllers
{
    public class Venta : Controller
    {
      

        [HttpPost]
        public ActionResult ProductoGetAll(ML.Producto producto)
        {
            ML.Result result = BL.Producto.ProductoGetByIdDepartamento(producto.Departamento.IdDepartamento);
            ML.Result resultDepartamento = BL.Departamento.GetByIdArea(producto.Departamento.Area.IdArea);
            ML.Result resultArea = BL.Area.GetAll();

            producto.Departamento.Departamentos = resultDepartamento.Objects;
            producto.Departamento.Area.Areas = resultArea.Objects;
            producto.Productos = result.Objects;

            return View("ProductoGetAll", producto);
        }

        [HttpGet]
        public ActionResult ProductoGetAll()
        {

            ML.Result result = BL.Producto.GetAll();
            ML.Producto producto = new ML.Producto();
            ML.Result resultDepartamento = BL.Departamento.GetAll();
            ML.Result resultAreas = BL.Area.GetAll();

            producto.Departamento = new ML.Departamento();
            producto.Departamento.Area = new ML.Area();

            if (result.Correct)
            {

                producto.Departamento.Departamentos = resultDepartamento.Objects;
                producto.Departamento.Area.Areas = resultAreas.Objects;
                //producto.Productos = result.Objects;
                return View(producto);
            }
            else
            {
                ViewBag.Message = "Ocurrió un error al obtener la información" + result.ErrorMessage;
                return PartialView("ValidationModal");
            }

        }// termina getall

        //[HttpPost]
        //public IActionResult ProductoGetAll(ML.Producto productoEntrada)
        //{
        //    ML.Result result = BL.Producto.GetAll();
        //    if (result.Correct)
        //    {
        //        ML.Producto producto = new ML.Producto();
        //        producto.Productos = result.Objects;

        //        return View(producto);
        //    }
        //    else
        //    {
        //        return View();
        //    }
        //}

        //[HttpGet]
        //public IActionResult ProductoGetAll()
        //{

        //    ML.Result result = BL.Producto.GetAll();
        //    ML.Producto producto = new ML.Producto();
        //    ML.Result  resultdepartamento = BL.Departamento.GetAll();
        //    ML.Result resultareas = BL.Area.GetAll();

        //    producto.Departamento = new ML.Departamento();
        //    producto.Departamento.Area = new ML.Area();

        //    if (result.Correct)
        //    {

        //        producto.Departamento.Departamentos = resultdepartamento.Objects;
        //        producto.Departamento.Area.Areas = resultareas.Objects;
        //        //producto.productos = result.objects;
        //        return View(producto);
        //    }
        //    else
        //    {
        //        ViewBag.message = "ocurrió un error al obtener la información" + result.ErrorMessage;
        //        return PartialView("validationmodal");
        //    }

        //}// termina getall

        public JsonResult GetDepartamento(int IdArea)
        {
            ML.Departamento depto = new ML.Departamento();

            depto.IdDepartamento = 0;
            depto.Nombre = "seleccione una opción";

            var result = BL.Departamento.GetByIdArea(IdArea);
            result.Objects.Insert(0, depto);
            //JsonRequestBehavior.allowget
            return Json(result.Objects);


        }


        [HttpGet]
        public IActionResult Carrito(int? IdProducto)//int IdProducto
        {
            ML.Result result = new ML.Result();
            result.Objects = new List<object>();

            ML.Result Salida = new ML.Result();
            Salida.Objects = new List<object>();
            if (IdProducto != null)
            {
                //Session["Prueba"] = IdProducto;
                /// HttpContext.Session.SetString("Usuario", usuarioLog.Usuario1);
                //S  HttpContext.Session["Pruebaeba"]

               // httpContextAccessor.HttpContext.Session.Get


                if (HttpContext.Session.GetString("Carrito") == null)
                { //Inicia sesion para agregar producto al carrito 

                    ML.VentaProducto ventaProducto = new ML.VentaProducto();

                    ventaProducto.Producto = new ML.Producto();
                    ventaProducto.Producto.IdProducto = IdProducto.Value;

                    ventaProducto.Cantidad = 1;

                    ML.Result resultProducto = BL.Producto.GetById(IdProducto.Value);

                    if (resultProducto.Correct)
                    {
                        ventaProducto.Producto = (ML.Producto)resultProducto.Object;

                        //result.Objects = new List<object>();
                        Salida.Objects.Add(ventaProducto);
                    }
                   // byte[] byteArray = result as byte[]

                        //string prueba = result.ToString();
                    
                    //byte[] byteArray = ObjToByteArray((object)result);
                    HttpContext.Session.SetString("Carrito", JsonSerializer.Serialize(Salida));
                    //Session["Carrito"] = result.Objects;
                }

                else
                {// comprobar si ya existe informacion en el carrito

                    string temp = HttpContext.Session.GetString("Carrito");
                    //result = (ML.Result)temp; 
                    result = JsonSerializer.Deserialize<ML.Result>(temp)!;

                    bool Existe = false;
                    var indice = 0; //variable para el index
                    
                    foreach (var temp2 in result.Objects)
                    {// foreach que recorre el objeto venta producto

                        ML.VentaProducto ventaProducto = new ML.VentaProducto();
                        ventaProducto = JsonSerializer.Deserialize<ML.VentaProducto>(temp2.ToString())!;
                        Salida.Objects.Add(ventaProducto);
                        if (ventaProducto.Producto.IdProducto == IdProducto)
                        {// if que compara el id de el producto con el de ventaproducto

                            Existe = true;
                            indice = result.Objects.IndexOf(ventaProducto);//index 

                        }
                    }

                    if (Existe == true)
                    {
                        foreach (ML.VentaProducto ventaProducto in result.Objects)
                        {
                            ventaProducto.Cantidad = ventaProducto.Cantidad + 1;//contador 
                        }

                    }

                    else
                    {

                        ML.VentaProducto ventaProducto = new ML.VentaProducto();

                        ventaProducto.Producto = new ML.Producto();
                        ventaProducto.Producto.IdProducto = IdProducto.Value;
                        ventaProducto.Cantidad = 1;


                        ML.Result resultProducto = BL.Producto.GetById(IdProducto.Value);

                        if (resultProducto.Correct)
                        {
                            ventaProducto.Producto = (ML.Producto)resultProducto.Object;

                            Salida.Objects.Add(ventaProducto);

                        }
                        //

                        
                          //string temp2 =   JsonSerializer.Serialize(result);
                       // byte[] byteArray = ObjToByteArray((object)result);
                       // object o = ByteArrayToObj(byteArray);

                        //ML.Result desCoche = (ML.Result)o;
                        //  HttpContext.Session.String("Carrito") = result.Objects;
                        // HttpContext.Session.GetString("Carrito")

                        HttpContext.Session.SetString("Carrito", JsonSerializer.Serialize(Salida));
                        //Session["Carrito"] = result.Objects;

                    }

                }
            }

            return View(Salida);
        }



        public IActionResult Sumar(int IdProducto)
        {
            ML.Result result = new ML.Result();
            ML.Result resultSalida = new ML.Result();
            resultSalida.Objects = new List<object>();
            string temp = HttpContext.Session.GetString("Carrito");
            // result = (ML.Result)temp;//unboxing de la lista

            //result = JsonConvert.DeserializeObject<ML.Result>(temp);
            result = JsonSerializer.Deserialize<ML.Result>(temp);


            foreach (object ventaProducto in result.Objects) //para comparar
            {
                ML.VentaProducto ventaProducto1 = new ML.VentaProducto();
                // 
                string temp2 = ventaProducto.ToString();

                ventaProducto1 = JsonSerializer.Deserialize<ML.VentaProducto>(temp2);
                
                if (ventaProducto1.Producto.IdProducto == IdProducto)
                {

                    ventaProducto1.Cantidad += 1;//aumenta la cantida

                }
                resultSalida.Objects.Add(ventaProducto1);
            }
            HttpContext.Session.SetString("Carrito", JsonSerializer.Serialize(resultSalida));
            return View("Carrito", resultSalida);
        }

        public IActionResult Restar(int IdProducto)
        {
            ML.Result result = new ML.Result();
            ML.Result resultSalida = new ML.Result();
            resultSalida.Objects = new List<object>();
            string temp = HttpContext.Session.GetString("Carrito");
            // result = (ML.Result)temp;//unboxing de la lista

            //result = JsonConvert.DeserializeObject<ML.Result>(temp);
            result = JsonSerializer.Deserialize<ML.Result>(temp);


            foreach (object ventaProducto in result.Objects) //para comparar
            {
                ML.VentaProducto ventaProducto1 = new ML.VentaProducto();
                // 
                string temp2 = ventaProducto.ToString();

                ventaProducto1 = JsonSerializer.Deserialize<ML.VentaProducto>(temp2);

                if (ventaProducto1.Producto.IdProducto == IdProducto)
                {

                    ventaProducto1.Cantidad -= 1;//aumenta la cantida

                }
                resultSalida.Objects.Add(ventaProducto1);
            }
            HttpContext.Session.SetString("Carrito", JsonSerializer.Serialize(resultSalida));
            return View("Carrito", resultSalida);
        }



        // metodo que le agrega 1 a la cantidad del producto en la lista de venta 

        //public IActionResult Restar(int IdProducto)
        //{
        //    ML.Result result = new ML.Result();

        //    result.Objects = (List<Object>)Session["Carrito"];//unboxing de la lista

        //    foreach (ML.VentaProducto ventaProducto in result.Objects) //para comparar
         //if (ventaProducto.Producto.IdProducto == IdProducto)
        //        {
        //            ventaProducto.Cantidad -= 1;//aumenta la cantidad
        //        }
        //    }
        //    return View("Carrito", result);     //    {

        //  
        //}//metodo que resta la cantidad de un producto en la lista de venta 

        public IActionResult Eliminar(int IdProducto)
        {
            ML.Result result = new ML.Result();
            ML.Result salida = new ML.Result(); 
            salida.Objects = new List<object>();
            //  result.Objects = (List<Object>)Session["Carrito"];
            string temp = HttpContext.Session.GetString("Carrito");
            // result = (ML.Result)temp;//unboxing de la lista

            //result = JsonConvert.DeserializeObject<ML.Result>(temp);
            result = JsonSerializer.Deserialize<ML.Result>(temp);

            var indice = 0; //variable para el index

            foreach (var temp2 in result.Objects)
            {// foreach que recorre el objeto venta producto


                ML.VentaProducto ventaProducto = new ML.VentaProducto();
                ventaProducto = JsonSerializer.Deserialize<ML.VentaProducto>(temp2.ToString())!;
                salida.Objects.Add(ventaProducto);
                if (ventaProducto.Producto.IdProducto == IdProducto)
                {// if que compara el id de el producto con el de ventaproducto

                    indice = result.Objects.IndexOf(temp2);//index 

                }
            }

            salida.Objects.RemoveAt(indice);
            //  Session["Carrito"] = result.Objects;
            HttpContext.Session.SetString("Carrito", JsonSerializer.Serialize(salida));

            return View("Carrito", salida);
        }// metodo que elimina un producto de la lista de venta 

        public decimal GetTotal(List<object> Objects)
        {
            decimal Total = 0;

            foreach (ML.VentaProducto ventaProducto in Objects)
            {
                Total += ventaProducto.Producto.PrecioUnitario * ventaProducto.Cantidad;
            }

            return Total;
        } // metodo que devuelve la suma del total de los productos comprados 
        public IActionResult Procesar()
        {
            ML.Result result = new ML.Result();
            ML.Result salida = new ML.Result();
            salida.Objects = new List<object>();
            //result.Objects = (List<Object>)Session["Carrito"];
            string temp = HttpContext.Session.GetString("Carrito");
            // result = (ML.Result)temp;//unboxing de la lista

            //result = JsonConvert.DeserializeObject<ML.Result>(temp);
            result = JsonSerializer.Deserialize<ML.Result>(temp);

            //des seralizar los objetosw 

            foreach(var desSerializar in result.Objects)
            {
                string cadenaTemporalSerializada = desSerializar.ToString();
                //ML.Venta venta1 = new ML.Venta();
                ML.VentaProducto ventaProducto = new ML.VentaProducto();
                ventaProducto = JsonSerializer.Deserialize<ML.VentaProducto>(cadenaTemporalSerializada);
             ML.Result stock = new ML.Result();
                stock = BL.Producto.RestarStock(ventaProducto.Cantidad, ventaProducto.Producto.IdProducto);
                //ventaProducto.
                salida.Objects.Add(ventaProducto);

            }

            ML.Venta venta = new ML.Venta();

            venta.Usuario = new ML.Usuario();

            // se obtiene el id del usuario4
            ClaimsPrincipal principal = new ClaimsPrincipal();
            //  string id = principal.FindFirstValue.ToString();
            venta.Usuario.IdUsuario = User.getUserId();


            //venta.Usuario.UserName = "";
            //string  userId = User.Identity.
            //ClaimsPrincipal user;

            //ClaimsPrincipal currentUser = new ClaimsPrincipal();


            //var ID = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier);



            //string userName = HttpContext.User.Identity.IsAuthenticated.

            //

           // ventaProducto.Venta = new ML.Venta();
           // ventaProducto.Venta.Fecha = DateTime.Now;
           venta.Fecha = DateTime.Now;

            venta.MetodoPago = new ML.MetodoPago();
            venta.MetodoPago.IdMetodoPago = 1;

            venta.Total = GetTotal(salida.Objects);

            result = BL.Venta.Add(venta,salida.Objects);//guardar el resultado de una vartiable en un metodo 

           // venta.IdVenta = ((ML.Venta)result.Object).IdVenta;// unboxing 

          //  return RedirectToAction("GetById", "VentaProducto", new { IdVenta = venta.IdVenta });

            return RedirectToAction("Index", "Home");

        }//metodo que devuelve el detalle de los productos elegidos para comprar 

        public IActionResult ModalCompra()
        {
            //aqui se tiene que obtener y enviar la informacion de que usuario es el que compra 
            //User.Identity.GetHashCode();

            ViewBag.Message = "¿Deseas finalizar tu compra?";
            return PartialView("ValidationModal");
        }

        //public static string getUserId(this ClaimsPrincipal user)
        //{
        //    if (!user.Identity.IsAuthenticated)
        //        return null;

        //    ClaimsPrincipal currentUser = user;
        //    return currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
        //}
        //private static byte[] ObjToByteArray(object o)
        //{
        //    if (o == null)
        //    {
        //        return null;
        //    }

        //    BinaryFormatter bf = new BinaryFormatter();
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        bf.Serialize(ms, o);
        //        return ms.ToArray();
        //    }
        //}

        //private static object ByteArrayToObj(byte[] byteArray)
        //{
        //    if (byteArray == null)
        //    {
        //        return null;
        //    }

        //    BinaryFormatter bf = new BinaryFormatter();
        //    using (MemoryStream ms = new MemoryStream(byteArray))
        //    {
        //        return (object)bf.Deserialize(ms);
        //    }
        //}

    }
}
