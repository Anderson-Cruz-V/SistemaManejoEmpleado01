using Microsoft.AspNetCore.Mvc;

namespace SistemaManejoEmpleados.Controllers
{
    public class EmpleadosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
