using System.ComponentModel.DataAnnotations;

namespace TendalProject.Web.ViewModels.Categoria
{
    public class RegistrarCategoriaViewModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio."),
            RegularExpression("^[a-zA-ZáéíóúÁÉÍÓÚñÑ\\s]+$", ErrorMessage = "El nombre solo puede contener letras y espacios."),
            StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string Nombre { get; set; } = null!;
        [Required(ErrorMessage = "La descripción es obligatoria."),
            RegularExpression("^[a-zA-ZáéíóúÁÉÍÓÚñÑ\\s]+$", ErrorMessage = "La descripción solo puede contener letras y espacios."),
            StringLength(250, ErrorMessage = "La descripción no puede exceder los 250 caracteres.")]
        public string Descripcion { get; set; } = null!;
    }
}
