using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
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
            var empleados = await _context.Empleados.ToListAsync();
            ViewBag.DepNombres = await _context.Departamentos.ToDictionaryAsync(d => d.Id, d => d.Nombre);
            ViewBag.CargoTitulos = await _context.Cargos.ToDictionaryAsync(c => c.Id, c => c.Titulo);
            return View(empleados);
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
            ViewBag.DepartamentoId = new SelectList(_context.Departamentos, "Id", "Nombre");
            ViewBag.CargoId = new SelectList(_context.Cargos, "Id", "Titulo");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Empleado empleado)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.DepartamentoId = new SelectList(_context.Departamentos, "Id", "Nombre", empleado.DepartamentoId);
                ViewBag.CargoId = new SelectList(_context.Cargos, "Id", "Titulo", empleado.CargoId);
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

            ViewBag.DepartamentoId = new SelectList(_context.Departamentos, "Id", "Nombre", empleado.DepartamentoId);
            ViewBag.CargoId = new SelectList(_context.Cargos, "Id", "Titulo", empleado.CargoId);
            return View(empleado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Empleado empleado)
        {
            if (id != empleado.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.DepartamentoId = new SelectList(_context.Departamentos, "Id", "Nombre", empleado.DepartamentoId);
                ViewBag.CargoId = new SelectList(_context.Cargos, "Id", "Titulo", empleado.CargoId);
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

            var culture = CultureInfo.CurrentCulture;
            var sep = culture.TextInfo.ListSeparator;

            string Esc(string s)
            {
                s = s ?? string.Empty;
                var needsQuote = s.Contains(sep) || s.Contains('"') || s.Contains('\n') || s.Contains('\r');
                s = s.Replace("\"", "\"\"");
                return needsQuote ? $"\"{s}\"" : s;
            }

            var sb = new StringBuilder();
            sb.AppendLine(string.Join(sep, new[]
            {
                "Id","Nombre","Departamento","Cargo","FechaInicio","Salario",
                "Estado","TiempoEnEmpresa","AFP","ARS","ISR"
            }));

            foreach (var e in empleados)
            {
                var dep = deps.TryGetValue(e.DepartamentoId, out var d) ? d : "";
                var cargo = cargos.TryGetValue(e.CargoId, out var c) ? c : "";
                var estado = e.Estado ? "Vigente" : "No vigente";

                var fila = new[]
                {
                    e.Id.ToString(culture),
                    Esc(e.Nombre),
                    Esc(dep),
                    Esc(cargo),
                    e.FechaInicio.ToString("dd/MM/yyyy", culture),
                    e.Salario.ToString("N2", culture),
                    Esc(estado),
                    Esc(e.TiempoEnEmpresa),
                    e.AFP.ToString("N2", culture),
                    e.ARS.ToString("N2", culture),
                    e.ISR.ToString("N2", culture)
                };

                sb.AppendLine(string.Join(sep, fila));
            }

            var utf8Bom = new UTF8Encoding(true);
            var preamble = utf8Bom.GetPreamble();
            var bodyBytes = utf8Bom.GetBytes(sb.ToString());
            var bytes = preamble.Concat(bodyBytes).ToArray();

            return File(bytes, "text/csv; charset=utf-8", "Empleados.csv");
        }
    }
}
