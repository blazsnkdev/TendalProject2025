using System.ComponentModel.DataAnnotations;

namespace TendalProject.Web.ViewModels.Articulo
{
    public class CalificarArticuloViewModel
    {
        public Guid ArticuloId { get; set; }
        public Guid ClienteId { get; set; }
        [Display(Name = "Asignar Puntuación")]
        [Required(ErrorMessage = "La puntuación es obligatoria")]
        [Range(1, 5, ErrorMessage = "La puntuación debe estar entre 1 y 5")]
        public int Puntuacion { get; set; }

        [Display(Name = "Comentarios")]
        [Required(ErrorMessage = "El comentario es obligatorio")]
        [StringLength(500, MinimumLength = 5,
            ErrorMessage = "El comentario debe tener entre 5 y 500 caracteres")]
        public string Comentario { get; set; } = string.Empty;
    }
}
