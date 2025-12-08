namespace TendalProject.Business.DTOs.Responses.Usuario
{
    public record ListarUsuarioResponse
    (
        Guid UsuarioId,
        string Email,
        string Password,
        bool Activo,
        DateTime? UltimaConexion
    );
}
