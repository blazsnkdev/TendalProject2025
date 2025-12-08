namespace TendalProject.Business.DTOs.Responses.Usuario
{
    public record DetalleUsuarioResponse
    (
        Guid UsuarioId,
        string Email,
        DateTime? UltimaConexion,
        int IntentosFallidos,
        bool Activo,    
        DateTime FechaCreacion,
        List<string> Roles
    );
}
