using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaManejoEmpleados.Models
{
    public class Empleado
    {
        public int Id { get; set; }

        [Display(Name = "Nombre Completo")]
        public string NombreCompleto { get; set; }

        public string Cedula { get; set; }
        public string Correo { get; set; }
        public DateTime FechaNacimiento { get; set; }

        public int DepartamentoId { get; set; }
        public Departamento Departamento { get; set; }

        public int CargoId { get; set; }
        public Cargo Cargo { get; set; }

        public DateTime FechaInicio { get; set; }

        public decimal Salario { get; set; }

        public bool Estado { get; set; }

        [NotMapped]
        public string TiempoEnEmpresa
        {
            get
            {
                var span = DateTime.Now - FechaInicio;
                return $"{span.Days / 365} años, {(span.Days % 365) / 30} meses";
            }
        }

        [NotMapped] public decimal AFP => Salario * 0.0287m;
        [NotMapped] public decimal ARS => Salario * 0.0304m;
        [NotMapped]
        public decimal ISR
        {
            get
            {
                if (Salario <= 40000) return 0;
                if (Salario <= 100000) return (Salario - 40000) * 0.15m;
                return (Salario - 100000) * 0.20m + (60000 * 0.15m);
            }
        }
    }
}

