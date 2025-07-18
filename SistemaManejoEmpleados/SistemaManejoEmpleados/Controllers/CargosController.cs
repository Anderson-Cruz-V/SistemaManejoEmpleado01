using Microsoft.AspNetCore.Mvc;

namespace SistemaManejoEmpleados.Controllers
{
    public class CargosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
