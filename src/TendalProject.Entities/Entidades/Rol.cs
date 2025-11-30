namespace TendalProject.Entities.Entidades
{
    public class Rol
    {
        public Guid RolId { get; set; }
        public string Nombre { get; set; } = null!;//TODO: esto hace que no sea null
        public string? Descripcion { get; set; }
        public ICollection<UsuarioRol> UsuariosRoles { get; set; } = new List<UsuarioRol>();
    }
}
