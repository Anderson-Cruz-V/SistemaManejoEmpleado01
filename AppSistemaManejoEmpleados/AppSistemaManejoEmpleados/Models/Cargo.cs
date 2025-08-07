using System.ComponentModel.DataAnnotations;

namespace AppSistemaManejoEmpleados.Models
{
    public class Cargo
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El título del cargo es obligatorio")]
        public string Titulo { get; set; } = string.Empty;
    }
}
