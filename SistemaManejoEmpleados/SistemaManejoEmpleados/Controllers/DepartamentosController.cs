using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaManejoEmpleados.Data;
using SistemaManejoEmpleados.Models;

namespace SistemaManejoEmpleados.Controllers
{
    public class DepartamentosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DepartamentosController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        public async Task<IActionResult> Lista()
        {
            var departamentos = await _context.Departamentos.ToListAsync();
            return View(departamentos);
        }

        
        public IActionResult Agregar()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Agregar(Departamento departamento)
        {
            if (ModelState.IsValid)
            {
                _context.Departamentos.Add(departamento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Lista));
            }
            return View(departamento);
        }

        public async Task<IActionResult> Editar(int id)
        {
            var departamento = await _context.Departamentos.FindAsync(id);
            if (departamento == null) return NotFound();
            return View(departamento);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Departamento departamento)
        {
            if (id != departamento.Id) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(departamento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Lista));
            }
            return View(departamento);
        }

      
        public async Task<IActionResult> Eliminar(int id)
        {
            var departamento = await _context.Departamentos
                .FirstOrDefaultAsync(d => d.Id == id);
            if (departamento == null) return NotFound();
            return View(departamento);
        }

      
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var departamento = await _context.Departamentos.FindAsync(id);
            if (departamento != null)
            {
                _context.Departamentos.Remove(departamento);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Lista));
        }
    }
}
