namespace TendalProject.Business.DTOs.Responses.Auth
{
    public record LoginValidoResponse
    (
        Guid sesionId,
        Guid usuarioId,
        Guid? clienteId,
        string nombre,
        DateTime? ultimoLogin,
        DateTime inicioSesion,
        string[] roles
        );
}
