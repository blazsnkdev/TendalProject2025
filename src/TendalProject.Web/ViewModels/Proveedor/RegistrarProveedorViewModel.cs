using System.ComponentModel.DataAnnotations;

namespace TendalProject.Web.ViewModels.Proveedor
{
    public class RegistrarProveedorViewModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [RegularExpression("^[a-zA-ZáéíóúÁÉÍÓÚñÑ\\s]+$", ErrorMessage = "El nombre solo puede contener letras y espacios.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;
        [Required(ErrorMessage = "La razón social es obligatoria.")]
        [Display(Name = "Razón Social")]
        [StringLength(150, ErrorMessage = "La razón social no puede exceder los 150 caracteres.")]
        public string RazonSocial { get; set; } = string.Empty;
        [Required(ErrorMessage = "El RUC es obligatorio.")]
        [Display(Name = "Número de RUC")]
        [RegularExpression("^[0-9]{11}$", ErrorMessage = "El RUC debe contener exactamente 11 dígitos.")]
        public string Ruc { get; set; } = string.Empty;
        [Required(ErrorMessage = "El nombre del contacto es obligatorio.")]
        [Display(Name = "Número de contacto")]
        [RegularExpression("^[a-zA-ZáéíóúÁÉÍÓÚñÑ\\s]+$", ErrorMessage = "El contacto solo puede contener letras y espacios.")]
        [StringLength(120, ErrorMessage = "El contacto no puede exceder los 120 caracteres.")]
        public string Contacto { get; set; } = string.Empty;
        [Required(ErrorMessage = "El número de teléfono es obligatorio.")]
        [Display(Name = "Número de telefono")]
        [RegularExpression("^[0-9]{7,12}$", ErrorMessage = "El teléfono debe contener entre 7 y 12 dígitos.")]
        public string Telefono { get; set; } = string.Empty;
        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "Ingrese un email válido.")]
        [Display(Name = "Correo Electronico")]
        [StringLength(150, ErrorMessage = "El email no puede exceder los 150 caracteres.")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "La dirección es obligatoria.")]
        [Display(Name = "Dirección actual")]
        [StringLength(200, ErrorMessage = "La dirección no puede exceder los 200 caracteres.")]
        public string Direccion { get; set; } = string.Empty;
    }
}
