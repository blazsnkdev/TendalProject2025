namespace TendalProject.Entities.Entidades
{
    public class Usuario
    {
        public Guid UsuarioId { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public int MyProperty { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int? TrabajadorId { get; set; }
        public Trabajador? Trabajador { get; set; }
        public int? ClienteId { get; set; }
        public Cliente? Cliente { get; set; }
        public bool Activo { get; set; }
        public DateTime? UltimaConexion { get; set; }
        public int IntentosFallidos { get; set; }//TODO: che esto es para el futuro para hacer un conteo de intentos fallidos de conexion
    }
}

