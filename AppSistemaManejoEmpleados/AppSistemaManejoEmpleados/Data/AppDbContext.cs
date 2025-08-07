using Microsoft.EntityFrameworkCore;
using AppSistemaManejoEmpleados.Models;

namespace AppSistemaManejoEmpleados.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Cargo> Cargos { get; set; }
    }
}
