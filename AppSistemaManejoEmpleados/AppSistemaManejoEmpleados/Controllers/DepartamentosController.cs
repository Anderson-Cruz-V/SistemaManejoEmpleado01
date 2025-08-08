using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppSistemaManejoEmpleados.Data;
using AppSistemaManejoEmpleados.Models;

namespace AppSistemaManejoEmpleados.Controllers
{
    public class DepartamentosController : Controller
    {
        private readonly AppDbContext _context;

        public DepartamentosController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
            => View(await _context.Departamentos.ToListAsync());

        public IActionResult Crear() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Departamento departamento)
        {
            if (!ModelState.IsValid) return View(departamento);
            _context.Add(departamento);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null) return NotFound();
            var dep = await _context.Departamentos.FindAsync(id);
            if (dep == null) return NotFound();
            return View(dep);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Departamento departamento)
        {
            if (id != departamento.Id) return NotFound();
            if (!ModelState.IsValid) return View(departamento);
            _context.Update(departamento);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Detalles(int? id)
        {
            if (id == null) return NotFound();
            var dep = await _context.Departamentos.FirstOrDefaultAsync(m => m.Id == id);
            if (dep == null) return NotFound();
            return View(dep);
        }

        public async Task<IActionResult> Eliminar(int? id)
        {
            if (id == null) return NotFound();
            var dep = await _context.Departamentos.FirstOrDefaultAsync(m => m.Id == id);
            if (dep == null) return NotFound();
            return View(dep);
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var dep = await _context.Departamentos.FindAsync(id);
            if (dep == null) return NotFound();
            _context.Departamentos.Remove(dep);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ExportarCsv()
        {
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
            sb.AppendLine(string.Join(sep, new[] { "Id", "Nombre" }));

            foreach (var d in _context.Departamentos.ToList())
                sb.AppendLine(string.Join(sep, new[] { d.Id.ToString(culture), Esc(d.Nombre) }));

            var utf8Bom = new UTF8Encoding(true);
            var preamble = utf8Bom.GetPreamble();
            var bodyBytes = utf8Bom.GetBytes(sb.ToString());
            var bytes = preamble.Concat(bodyBytes).ToArray();

            return File(bytes, "text/csv; charset=utf-8", "Departamentos.csv");
        }
    }
}
