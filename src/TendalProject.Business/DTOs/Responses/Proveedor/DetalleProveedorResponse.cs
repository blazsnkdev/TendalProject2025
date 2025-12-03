namespace TendalProject.Business.DTOs.Responses.Proveedor
{
    public record DetalleProveedorResponse
    (
        Guid ProveedorId,
        string Nombre,
        string RazonSocial,
        string Ruc,
        string? Contacto,
        string Telefono,
        string? Email,
        string? Direccion,
        string Estado,
        DateTime FechaRegistro
    );
}
