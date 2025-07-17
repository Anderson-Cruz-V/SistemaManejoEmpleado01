namespace SistemaManejoEmpleados.Models
{
    public class Empleado
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; }
        public string Cedula { get; set; }
        public string Correo { get; set; }
        public DateTime FechaNacimiento { get; set; }

        public int CargoId { get; set; }
        public Cargo Cargo { get; set; }
    }
}
