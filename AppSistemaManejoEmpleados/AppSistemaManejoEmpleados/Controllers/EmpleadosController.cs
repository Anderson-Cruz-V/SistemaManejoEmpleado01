using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppSistemaManejoEmpleados.Data;
using AppSistemaManejoEmpleados.Models;

namespace AppSistemaManejoEmpleados.Controllers
{
    public class EmpleadosController : Controller
    {
        private readonly AppDbContext _context;
        public EmpleadosController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var lista = await _context.Empleados.ToListAsync();
            return View(lista);
        }

        public async Task<IActionResult> Detalles(int? id)
        {
            if (id == null) return NotFound();
            var empleado = await _context.Empleados.FirstOrDefaultAsync(m => m.Id == id);
            if (empleado == null) return NotFound();
            return View(empleado);
        }

        public IActionResult Crear()
        {
            ViewData["DepartamentoId"] = new SelectList(_context.Departamentos, "Id", "Nombre");
            ViewData["CargoId"] = new SelectList(_context.Cargos, "Id", "Titulo");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Empleado empleado)
        {
            if (!ModelState.IsValid)
            {
                ViewData["DepartamentoId"] = new SelectList(_context.Departamentos, "Id", "Nombre", empleado.DepartamentoId);
                ViewData["CargoId"] = new SelectList(_context.Cargos, "Id", "Titulo", empleado.CargoId);
                return View(empleado);
            }

            _context.Add(empleado);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null) return NotFound();
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null) return NotFound();

            ViewData["DepartamentoId"] = new SelectList(_context.Departamentos, "Id", "Nombre", empleado.DepartamentoId);
            ViewData["CargoId"] = new SelectList(_context.Cargos, "Id", "Titulo", empleado.CargoId);
            return View(empleado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Empleado empleado)
        {
            if (id != empleado.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                ViewData["DepartamentoId"] = new SelectList(_context.Departamentos, "Id", "Nombre", empleado.DepartamentoId);
                ViewData["CargoId"] = new SelectList(_context.Cargos, "Id", "Titulo", empleado.CargoId);
                return View(empleado);
            }

            _context.Update(empleado);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Eliminar(int? id)
        {
            if (id == null) return NotFound();
            var empleado = await _context.Empleados.FirstOrDefaultAsync(m => m.Id == id);
            if (empleado == null) return NotFound();
            return View(empleado);
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null) return NotFound();
            _context.Empleados.Remove(empleado);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ExportarCsv()
        {
            var deps = _context.Departamentos.ToDictionary(d => d.Id, d => d.Nombre);
            var cargos = _context.Cargos.ToDictionary(c => c.Id, c => c.Titulo);
            var empleados = _context.Empleados.ToList();

            var sb = new StringBuilder();
            sb.AppendLine("Id,Nombre,Departamento,Cargo,FechaInicio,Salario,Estado,TiempoEnEmpresa,AFP,ARS,ISR");

            foreach (var e in empleados)
            {
                var nombreDep = deps.ContainsKey(e.DepartamentoId) ? deps[e.DepartamentoId] : "";
                var tituloCargo = cargos.ContainsKey(e.CargoId) ? cargos[e.CargoId] : "";
                var estadoTxt = e.Estado ? "Vigente" : "Inactivo";
                sb.AppendLine($"{e.Id},{e.Nombre},{nombreDep},{tituloCargo},{e.FechaInicio:yyyy-MM-dd},{e.Salario},{estadoTxt},{e.TiempoEnEmpresa},{e.AFP},{e.ARS},{e.ISR}");
            }

            byte[] buffer = Encoding.UTF8.GetBytes(sb.ToString());
            return File(buffer, "text/csv", "Empleados.csv");
        }
    }
}
