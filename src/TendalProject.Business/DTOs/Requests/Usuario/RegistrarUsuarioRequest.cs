namespace TendalProject.Business.DTOs.Requests.Usuario
{
    public record RegistrarUsuarioRequest
    (
        string Email,
        string Password,
        string ConfirmarPassword
    );
}
