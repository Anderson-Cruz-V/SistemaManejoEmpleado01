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
        {
            var lista = await _context.Departamentos.ToListAsync();
            return View(lista);
        }

        public async Task<IActionResult> Detalles(int? id)
        {
            if (id == null) return NotFound();
            var departamento = await _context.Departamentos.FirstOrDefaultAsync(m => m.Id == id);
            if (departamento == null) return NotFound();
            return View(departamento);
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Departamento departamento)
        {
            if (ModelState.IsValid)
            {
                _context.Add(departamento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(departamento);
        }

        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null) return NotFound();
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
                return RedirectToAction(nameof(Index));
            }
            return View(departamento);
        }

        public async Task<IActionResult> Eliminar(int? id)
        {
            if (id == null) return NotFound();
            var departamento = await _context.Departamentos.FirstOrDefaultAsync(m => m.Id == id);
            if (departamento == null) return NotFound();
            return View(departamento);
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var departamento = await _context.Departamentos.FindAsync(id);
            if (departamento == null) return NotFound();
            _context.Departamentos.Remove(departamento);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
