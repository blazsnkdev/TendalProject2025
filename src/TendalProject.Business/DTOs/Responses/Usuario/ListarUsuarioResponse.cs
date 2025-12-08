namespace TendalProject.Business.DTOs.Responses.Usuario
{
    public record ListarUsuarioResponse
    (
        Guid UsuarioId,
        string Email,
        bool Activo,
        DateTime? UltimaConexion
    );
}
