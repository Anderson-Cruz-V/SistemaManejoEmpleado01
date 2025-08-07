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
        {
            var lista = await _context.Cargos.ToListAsync();
            return View(lista);
        }

        public async Task<IActionResult> Detalles(int? id)
        {
            if (id == null) return NotFound();
            var cargo = await _context.Cargos.FirstOrDefaultAsync(m => m.Id == id);
            if (cargo == null) return NotFound();
            return View(cargo);
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Cargo cargo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cargo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cargo);
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
            if (ModelState.IsValid)
            {
                _context.Update(cargo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
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
    }
}
