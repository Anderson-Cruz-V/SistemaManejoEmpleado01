using Microsoft.AspNetCore.Mvc;
using AppSistemaManejoEmpleados.Data;
using System.Linq;

namespace AppSistemaManejoEmpleados.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context) { _context = context; }

        public IActionResult Index()
        {
            ViewBag.CantEmpl = _context.Empleados.Count();
            ViewBag.CantDep = _context.Departamentos.Count();
            ViewBag.CantCarg = _context.Cargos.Count();
            return View();
        }
    }
}
