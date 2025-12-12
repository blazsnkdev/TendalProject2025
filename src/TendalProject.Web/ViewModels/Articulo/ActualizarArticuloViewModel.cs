using System.ComponentModel.DataAnnotations;

namespace TendalProject.Web.ViewModels.Articulo
{
    public class ActualizarArticuloViewModel
    {
        public Guid ArticuloId { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(200, ErrorMessage = "La descripción no puede superar los 200 caracteres.")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.01, 99999, ErrorMessage = "El precio debe ser mayor a 0.")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El precio oferta es obligatorio.")]
        [Range(0.00, 99999, ErrorMessage = "El precio oferta no es válido.")]
        public decimal? PrecioOferta { get; set; }

        [Required(ErrorMessage = "El stock es obligatorio.")]
        [Range(0, 999999, ErrorMessage = "El stock debe ser un número positivo.")]
        public int Stock { get; set; }

        public bool Destacado { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una categoría.")]
        public Guid CategoriaId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un proveedor.")]
        public Guid ProveedorId { get; set; }
    }
}
