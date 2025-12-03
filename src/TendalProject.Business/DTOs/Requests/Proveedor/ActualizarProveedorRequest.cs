namespace TendalProject.Business.DTOs.Requests.Proveedor
{
    public record ActualizarProveedorRequest
    (
        Guid ProveedorId,
        string Nombre,
        string RazonSocial,
        string Ruc,
        string Contacto,
        string Telefono,
        string Email,
        string Direccion
        );
}
