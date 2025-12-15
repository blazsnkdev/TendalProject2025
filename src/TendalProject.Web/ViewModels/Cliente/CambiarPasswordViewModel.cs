using System.ComponentModel.DataAnnotations;

namespace TendalProject.Web.ViewModels.Cliente
{
    public class CambiarPasswordViewModel
    {
        [Display(Name = "Contraseña Actual"),
            Required(ErrorMessage = "La contraseña actual es obligatoria."),
            MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        public string PasswordActual { get; set; } = string.Empty;
        [Display(Name = "Nueva Contraseña"),
            Required(ErrorMessage = "La nueva contraseña es obligatoria."),
            MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        public string NuevoPassword { get; set; } = string.Empty;
        [Display(Name = "Confirmar Contraseña"),
            Required(ErrorMessage = "La confirmación de la contraseña es obligatoria."),
            MinLength(6, ErrorMessage = "La confirmación de la contraseña debe tener al menos 6 caracteres."),
            Compare("NuevoPassword", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmarPassword { get; set; } = string.Empty;
    }
}
