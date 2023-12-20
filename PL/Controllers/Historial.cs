using BL;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace PL.Controllers
{
    public class Historial : Controller
    {
        public IActionResult MostrarHistorial()
        {
           // ML.Producto producto = new ML.Producto();
           ML.Venta venta   = new ML.Venta();
            ML.Result result = new ML.Result();
            ClaimsPrincipal principal = new ClaimsPrincipal();
            //  string id = principal.FindFirstValue.ToString();
            string idUsuario = User.getUserId();

            result = BL.Venta.GetByUsuario(idUsuario);

            venta.Ventas = result.Objects;

            


            return View(venta);
        }
    }
}
