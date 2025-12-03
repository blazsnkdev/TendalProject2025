using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace TendalProject.Web.ViewModels.Articulo
{
    public class RegistrarArticuloViewModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [RegularExpression("^[a-zA-ZáéíóúÁÉÍÓÚñÑ\\s]+$", ErrorMessage = "El nombre solo puede contener letras y espacios.")]
        [StringLength(120, ErrorMessage = "El nombre no puede exceder los 120 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(300, ErrorMessage = "La descripción no puede exceder los 300 caracteres.")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.10, 100000, ErrorMessage = "El precio debe ser mayor a 0.")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El stock es obligatorio.")]
        [Range(0, 100000, ErrorMessage = "El stock debe ser mayor o igual a 0.")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una categoría.")]
        public Guid CategoriaId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un proveedor.")]
        public Guid ProveedorId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una imagen.")]
        [DataType(DataType.Upload)]
        public IFormFile? Imagen { get; set; }
    }
}
