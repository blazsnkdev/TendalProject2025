namespace TendalProject.Entities.Entidades
{
    public class Usuario
    {
        public Guid UsuarioId { get; set; }
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; }=null!;
        public DateTime FechaCreacion { get; set; }
        public bool Activo { get; set; }
        public DateTime? UltimaConexion { get; set; }
        public int IntentosFallidos { get; set; }//TODO: che esto es para el futuro para hacer un conteo de intentos fallidos de conexion
        public ICollection<UsuarioRol> UsuariosRoles { get; set; } = new List<UsuarioRol>();
        public Cliente? Cliente { get; set; }
    }
}

