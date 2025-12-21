namespace TendalProject.Business.DTOs.Responses.Cliente
{
    public record DetallePerfilClienteResponse
    (
        Guid ClienteId,
        string Nombre,
        string ApellidoPaterno,
        string ApellidoMaterno,
        string CorreoElectronico,
        string NumeroCelular,
        DateOnly FechaNacimiento,
        string Nivel
    );
}
