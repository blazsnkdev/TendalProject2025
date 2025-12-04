using System.ComponentModel.DataAnnotations;

namespace TendalProject.Web.ViewModels.Articulo
{
    public class ActualizarImagenArticuloViewModel
    {
        public Guid ArticuloId { get; set; }
        [Required]
        [DataType(DataType.Upload)]
        public IFormFile? Imagen { get; set; }
    }
}
