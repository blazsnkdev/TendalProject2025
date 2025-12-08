namespace TendalProject.Web.ViewModels.Usuario
{
    public class DetalleUsuarioViewModel
    {
        public Guid UsuarioId { get; set; }
        public string Email { get; set; } = string.Empty;
        public DateTime? UltimaConexion { get; set; }
        public int IntentosFallidos { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}
