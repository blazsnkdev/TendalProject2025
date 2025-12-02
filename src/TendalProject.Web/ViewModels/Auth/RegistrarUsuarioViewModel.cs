using System.ComponentModel.DataAnnotations;

namespace TendalProject.Web.ViewModels.Auth
{
    public class RegistrarUsuarioViewModel
    {
        [Display(Name = "Nombre"),
            Required(ErrorMessage = "El nombre es obligatorio."),
            MaxLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres."),
            RegularExpression("^[a-zA-ZáéíóúÁÉÍÓÚñÑ\\s]+$",
                ErrorMessage = "El nombre solo puede contener letras y espacios.")]
        public string Nombre { get; set; } = null!;

        [Display(Name = "Apellido Paterno"),
            Required(ErrorMessage = "El apellido paterno es obligatorio."),
            MaxLength(100, ErrorMessage = "El apellido paterno no puede exceder los 100 caracteres."),
            RegularExpression("^[a-zA-ZáéíóúÁÉÍÓÚñÑ\\s]+$",
                ErrorMessage = "El apellido paterno solo puede contener letras y espacios.")]
        public string ApellidoPaterno { get; set; } = null!;

        [Display(Name = "Apellido Materno"),
            Required(ErrorMessage = "El apellido materno es obligatorio."),
            MaxLength(100, ErrorMessage = "El apellido materno no puede exceder los 100 caracteres."),
            RegularExpression("^[a-zA-ZáéíóúÁÉÍÓÚñÑ\\s]+$",
                ErrorMessage = "El apellido materno solo puede contener letras y espacios.")]
        public string ApellidoMaterno { get; set; } = null!;

        [Display(Name = "Número de Celular"),
            Required(ErrorMessage = "El número de celular es obligatorio."),
            MaxLength(15, ErrorMessage = "El número de celular no puede exceder los 15 caracteres.")]
        public string NumeroCelular { get; set; } = null!;

        [Display(Name = "Fecha de Nacimiento"),
            Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        public DateOnly FechaNacimiento { get; set; }

        [Display(Name = "Email"),
            Required(ErrorMessage = "El email es obligatorio."),
            EmailAddress(ErrorMessage = "El email no es válido."),
            MaxLength(100, ErrorMessage = "El email no puede exceder los 100 caracteres.")]
        public string Email { get; set; } = null!;

        [Display(Name = "Contraseña"),
            Required(ErrorMessage = "La contraseña es obligatoria."),
            MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        public string Password { get; set; } = null!;

        [Display(Name = "Confirmar Contraseña"),
            Required(ErrorMessage = "La confirmación de la contraseña es obligatoria."),
            MinLength(6, ErrorMessage = "La confirmación de la contraseña debe tener al menos 6 caracteres."),
            Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmarPassword { get; set; } = null!;
    }
}
