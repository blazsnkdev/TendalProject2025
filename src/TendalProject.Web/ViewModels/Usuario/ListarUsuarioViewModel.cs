namespace TendalProject.Web.ViewModels.Usuario
{
    public class ListarUsuarioViewModel
    {
        public Guid UsuarioId { get; set; }
        public string Email { get; set; } = string.Empty;
        public bool Activo { get; set; }
        public DateTime? UltimaConexion { get; set; }
    }
}
