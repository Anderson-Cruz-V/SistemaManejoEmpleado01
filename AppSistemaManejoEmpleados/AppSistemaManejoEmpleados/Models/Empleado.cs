using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppSistemaManejoEmpleados.Models
{
    public class Empleado
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Seleccione un departamento")]
        public int DepartamentoId { get; set; }

        [Required(ErrorMessage = "Seleccione un cargo")]
        public int CargoId { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "El salario es obligatorio")]
        [Range(0, double.MaxValue, ErrorMessage = "Salario inválido")]
        public decimal Salario { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio")]
        public bool Estado { get; set; }

        // Propiedades calculadas (no mapeadas)
        [NotMapped]
        public string TiempoEnEmpresa
        {
            get
            {
                var diff = DateTime.Today - FechaInicio;
                int años = (int)(diff.Days / 365.25);
                int meses = (int)((diff.Days % 365.25) / 30);
                return $"{años} años, {meses} meses";
            }
        }

        [NotMapped]
        public decimal AFP => Salario * 0.0287m;

        [NotMapped]
        public decimal ARS => Salario * 0.0304m;

        [NotMapped]
        public decimal ISR
        {
            get
            {
                var baseGravable = Math.Max(0, Salario - 50000);
                return baseGravable * 0.10m;
            }
        }
    }
}
