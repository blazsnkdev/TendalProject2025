namespace TendalProject.Business.DTOs.Responses.Cliente
{
    public record DetalleClienteResponse
    (
        Guid ClienteId,
        string Nombre,
        string ApellidoPaterno,
        string ApellidoMaterno,
        string CorreoElectronico,
        string NumeroCelular,
        DateOnly FechaNacimiento,
        string Estado,
        DateTime? UltimaConexion,
        DateTime FechaCreacion,
        DateTime? UltimaModificacion,
        decimal MontoTotalGastado,
        string Nivel,
        int CantidadCompras
    );
}
