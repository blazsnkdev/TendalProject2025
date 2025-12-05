using System.ComponentModel.DataAnnotations;

namespace TendalProject.Web.ViewModels.Usuario
{
    public class RegistrarUsuarioViewModel
    {
        [Display(Name = "Correo electronico"),
            Required(ErrorMessage ="El correo electronico es obligatorio")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Contraseña"),
            Required(ErrorMessage = "La contraseña es obligatoria"),
            StringLength(100, ErrorMessage = "La contraseña debe tener al menos {6} caracteres.",
                MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Confirmar Contraseña"),
            Required(ErrorMessage = "La confirmación de la contraseña es obligatoria"),
            StringLength(100, ErrorMessage = "La confirmación de la contraseña debe tener al menos {6} caracteres.",
                MinimumLength = 6)]
        public string ConfirmarPassword { get; set; } = string.Empty;
    }
}
