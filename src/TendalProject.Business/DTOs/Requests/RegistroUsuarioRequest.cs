namespace TendalProject.Business.DTOs.Requests
{
    public record RegistroUsuarioRequest
    (
        string Nombre,
        string ApellidoPaterno,
        string ApellidoMaterno,
        string NumeroCelular,
        DateOnly FechaNacimiento,
        string Email,
        string Password,
        string ConfirmPassword
    );
}
