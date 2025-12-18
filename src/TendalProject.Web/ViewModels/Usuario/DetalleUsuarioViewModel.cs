using System.ComponentModel.DataAnnotations;

namespace TendalProject.Web.ViewModels.Usuario
{
    public class DetalleUsuarioViewModel
    {
        public Guid UsuarioId { get; set; }
        [Display(Name ="Correo elecontrico")]
        public string Email { get; set; } = string.Empty;
        [Display(Name ="Ultima conexión")]
        public DateTime? UltimaConexion { get; set; }
        [Display(Name ="Conteo de intentos fallidos")]
        public int IntentosFallidos { get; set; }
        public bool Activo { get; set; }
        [Display(Name ="Fecha registro")]
        public DateTime FechaCreacion { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}
