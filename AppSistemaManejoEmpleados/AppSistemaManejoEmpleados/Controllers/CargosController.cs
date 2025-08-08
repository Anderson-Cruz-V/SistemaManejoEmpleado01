using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppSistemaManejoEmpleados.Data;
using AppSistemaManejoEmpleados.Models;

namespace AppSistemaManejoEmpleados.Controllers
{
    public class CargosController : Controller
    {
        private readonly AppDbContext _context;

        public CargosController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
            => View(await _context.Cargos.ToListAsync());

        public IActionResult Crear() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Cargo cargo)
        {
            if (!ModelState.IsValid) return View(cargo);
            _context.Add(cargo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null) return NotFound();
            var cargo = await _context.Cargos.FindAsync(id);
            if (cargo == null) return NotFound();
            return View(cargo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Cargo cargo)
        {
            if (id != cargo.Id) return NotFound();
            if (!ModelState.IsValid) return View(cargo);
            _context.Update(cargo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Detalles(int? id)
        {
            if (id == null) return NotFound();
            var cargo = await _context.Cargos.FirstOrDefaultAsync(m => m.Id == id);
            if (cargo == null) return NotFound();
            return View(cargo);
        }

        public async Task<IActionResult> Eliminar(int? id)
        {
            if (id == null) return NotFound();
            var cargo = await _context.Cargos.FirstOrDefaultAsync(m => m.Id == id);
            if (cargo == null) return NotFound();
            return View(cargo);
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var cargo = await _context.Cargos.FindAsync(id);
            if (cargo == null) return NotFound();
            _context.Cargos.Remove(cargo);
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
            sb.AppendLine(string.Join(sep, new[] { "Id", "Titulo" }));

            foreach (var c in _context.Cargos.ToList())
                sb.AppendLine(string.Join(sep, new[] { c.Id.ToString(culture), Esc(c.Titulo) }));

            var utf8Bom = new UTF8Encoding(true);
            var preamble = utf8Bom.GetPreamble();
            var bodyBytes = utf8Bom.GetBytes(sb.ToString());
            var bytes = preamble.Concat(bodyBytes).ToArray();

            return File(bytes, "text/csv; charset=utf-8", "Cargos.csv");
        }
    }
}
