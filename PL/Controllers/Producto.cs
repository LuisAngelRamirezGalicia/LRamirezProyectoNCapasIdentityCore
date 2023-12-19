using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ML;

namespace PL.Controllers
{
    public class Producto : Controller
    {
        //falta update

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public ActionResult Form(int? idProducto)
        {
            


            ML.Producto producto = new ML.Producto();
            producto.Proveedor = new ML.Proveedor();
            producto.Departamento = new ML.Departamento();
            producto.Departamento.Area = new ML.Area();

            if(idProducto.HasValue)
            {
                ML.Result result = new ML.Result();
                result = BL.Producto.GetById(idProducto.Value);
                producto = (ML.Producto)result.Object;

            }
            return View(producto);
        }
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public IActionResult Form(ML.Producto producto,IFormFile Foto)
        {
            if (Foto != null)
            {
               producto.Foto = convertFileToByteArray(Foto);
            }




            if (producto.IdProducto == 0)
            {
          ML.Result result = BL.Producto.Add(producto);
                    if (result.Correct)
                    {
                        return PartialView("Modal");
                    }
            }
            else
            {
                ML.Result result = BL.Producto.Update(producto);
                if (result.Correct)
                {
                    return PartialView("Modal");
                }
            }
          
            return View();
        }

        [Authorize(Roles = "Administrador")]

        [HttpGet]
        public IActionResult GetAll(ML.Producto? productoEntrada)
        {ML.Result resultRellenar = BL.Producto.GetAll();
            ML.Result areas = BL.Area.GetAll();
            //ML.Result departamentos = BL.Departamento.GetByIdArea(1);


            if (productoEntrada.Nombre == null)
            {    
                if (resultRellenar.Correct)
                {
                    ML.Producto producto = new ML.Producto();
                    producto.Productos = resultRellenar.Objects;
                    producto.Departamento = new ML.Departamento();
                   // producto.Departamento.Departamentos = departamentos.Objects;
                    producto.Departamento.Area = new ML.Area();
                    producto.Departamento.Area.Areas = areas.Objects;
                        
                    return View(producto);
                }
                else
                {
                    return View();
                }
            }
            else
            {
                ML.Result result = BL.Producto.GetAllBusqueda(productoEntrada.Nombre);
                if (result.Correct)
                {
                    ML.Producto producto = new ML.Producto();
                    producto.Productos = result.Objects;

                    return View(producto);
                }
                else
                {
                    return View();
                }
            }
            

        }

        [Authorize(Roles = "Usuario")]

        [HttpGet]
        public IActionResult GetAllUsuario()
        {

            ML.Result result = BL.Producto.GetAll();
            if (result.Correct)
            {
                ML.Producto producto = new ML.Producto();
                producto.Productos = result.Objects;

                return View(producto);
            }
            else
            {
                return View();
            }

        }
        [Authorize(Roles = "Administrador")]
        [HttpGet]

        public IActionResult Delete(int idProducto)

        {
            ML.Result result = new ML.Result();
            result = BL.Producto.Delete(idProducto);
            if (result.Correct) { }
            return PartialView("Modal");

        }
        [Authorize(Roles = "Administrador,Usuario")]
        [HttpPost]
        public JsonResult GetDepartamento(int IdArea)
        {
            var result = BL.Departamento.GetByIdArea(IdArea);

            return Json(result.Objects);
        }

        public byte[] convertFileToByteArray(IFormFile foto)
        {
            //MemoryStream target = new MemoryStream(); 
            //foto.OpenReadStream.
            ////fuImagen.InputStream.CopyTo(target);
            //byte[] data = target.ToArray();
            //return data;
            
                byte[] buffer = new byte[foto.Length];
                var resultInBytes = ConvertToBytes(foto);
             
                Array.Copy(resultInBytes, buffer, resultInBytes.Length);


                return buffer;

            
        }
            private byte[] ConvertToBytes(IFormFile image)
            {
                using (var memoryStream = new MemoryStream())
                {
                    image.OpenReadStream().CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }

        //public ActionResult Upload(IFormFile file)
        //{
            

        //}

    }


    
}
