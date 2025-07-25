﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaManejoEmpleados.Data;
using SistemaManejoEmpleados.Models;

namespace SistemaManejoEmpleados.Controllers
{
    public class EmpleadosController : Controller
    {
        private readonly ApplicationDbContext _context;
        public EmpleadosController(ApplicationDbContext context) => _context = context;

        public async Task<IActionResult> Lista()
        {
            var empleados = await _context.Empleados
                .Include(e => e.Departamento)
                .Include(e => e.Cargo)
                .ToListAsync();
            return View(empleados);
        }

        public IActionResult Agregar()
        {
            ViewData["Departamentos"] = _context.Departamentos.ToList();
            ViewData["Cargos"] = _context.Cargos.ToList();
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Agregar(Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                _context.Empleados.Add(empleado);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Lista));
            }
            ViewData["Departamentos"] = _context.Departamentos.ToList();
            ViewData["Cargos"] = _context.Cargos.ToList();
            return View(empleado);
        }

        public async Task<IActionResult> Editar(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null) return NotFound();

            ViewData["Departamentos"] = _context.Departamentos.ToList();
            ViewData["Cargos"] = _context.Cargos.ToList();
            return View(empleado);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Empleado empleado)
        {
            if (id != empleado.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(empleado);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Lista));
            }
            ViewData["Departamentos"] = _context.Departamentos.ToList();
            ViewData["Cargos"] = _context.Cargos.ToList();
            return View(empleado);
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            var empleado = await _context.Empleados
                .Include(e => e.Departamento)
                .Include(e => e.Cargo)
                .FirstOrDefaultAsync(e => e.Id == id);
            if (empleado == null) return NotFound();
            return View(empleado);
        }

        [HttpPost, ActionName("Eliminar"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado != null)
            {
                _context.Empleados.Remove(empleado);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Lista));
        }
    }
}


