namespace SistemaManejoEmpleados.Models
{
    public class Departamento
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public ICollection<Cargo> Cargos { get; set; }
    }
}
