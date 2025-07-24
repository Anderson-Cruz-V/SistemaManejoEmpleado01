using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaManejoEmpleados.Data;
using SistemaManejoEmpleados.Models;

namespace SistemaManejoEmpleados.Controllers
{
    public class CargosController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CargosController(ApplicationDbContext context) => _context = context;

        public async Task<IActionResult> Lista()
        {
            var cargos = await _context.Cargos
                .Include(c => c.Departamento)
                .ToListAsync();
            return View(cargos);
        }

        public IActionResult Agregar()
        {
            ViewData["Departamentos"] = _context.Departamentos.ToList();
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Agregar(Cargo cargo)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Departamentos"] = _context.Departamentos.ToList();
                return View(cargo);
            }

            _context.Cargos.Add(cargo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Lista));
        }

        
        public async Task<IActionResult> Editar(int id)
        {
            var cargo = await _context.Cargos.FindAsync(id);
            if (cargo == null) return NotFound();

            ViewData["Departamentos"] = _context.Departamentos.ToList();
            return View(cargo);
        }

        
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Cargo cargo)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Departamentos"] = _context.Departamentos.ToList();
                return View(cargo);
            }

            _context.Cargos.Update(cargo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Lista));
        }

     
        public async Task<IActionResult> Eliminar(int id)
        {
            var cargo = await _context.Cargos
                .Include(c => c.Departamento)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (cargo == null) return NotFound();
            return View(cargo);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var cargo = await _context.Cargos.FindAsync(id);
            if (cargo != null)
            {
                _context.Cargos.Remove(cargo);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Lista));
        }
    }
}
