using Microsoft.AspNetCore.Mvc;

namespace SistemaManejoEmpleados.Controllers
{
    public class DepartamentosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
