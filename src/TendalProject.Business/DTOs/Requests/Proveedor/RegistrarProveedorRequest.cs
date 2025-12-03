namespace TendalProject.Business.DTOs.Requests.Proveedor
{
    public record RegistrarProveedorRequest
    (
        string Nombre,
        string RazonSocial,
        string Ruc,
        string Contacto,
        string Telefono,
        string Email,
        string Direccion
        );
}
