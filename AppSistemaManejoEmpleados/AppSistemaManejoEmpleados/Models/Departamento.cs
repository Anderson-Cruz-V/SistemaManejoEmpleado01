using System.ComponentModel.DataAnnotations;

namespace AppSistemaManejoEmpleados.Models
{
    public class Departamento
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del departamento es obligatorio")]
        public string Nombre { get; set; } = string.Empty;
    }
}

