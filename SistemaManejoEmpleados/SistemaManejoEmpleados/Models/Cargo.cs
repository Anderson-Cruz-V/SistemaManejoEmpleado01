namespace SistemaManejoEmpleados.Models
{
    public class Cargo
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public decimal Salario { get; set; }

        public int DepartamentoId { get; set; }
        public Departamento Departamento { get; set; }
        public ICollection<Empleado> Empleados { get; set; }
    }
}
