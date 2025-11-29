namespace TendalProject.Entities.Entidades
{
    public class Trabajador
    {
        public Guid TrabajadorId { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Dni { get; set; }
        public DateOnly FechaNacimiento { get; set; }
        public string Genero { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public string CorreoElectronico { get; set; }
        public DateOnly FechaContratacion { get; set; } 
        public Usuario Usuario { get; set; }
    }
}
