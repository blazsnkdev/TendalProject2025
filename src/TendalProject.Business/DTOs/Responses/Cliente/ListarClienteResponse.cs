namespace TendalProject.Business.DTOs.Responses.Cliente
{
    public record ListarClienteResponse
    (
        Guid ClienteId,
        string Nombre,
        string Apellidos,
        string CorreoElectronico,
        string NumeroCelular,
        string Estado
    );
}
