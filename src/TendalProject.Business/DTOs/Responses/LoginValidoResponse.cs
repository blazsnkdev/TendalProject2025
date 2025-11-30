namespace TendalProject.Business.DTOs.Responses
{
    public record LoginValidoResponse
    (
        Guid sesionId,
        Guid usuarioId,
        string nombre,
        DateTime ultimoLogin,
        DateTime inicioSesion,
        string[] roles
        );
}
