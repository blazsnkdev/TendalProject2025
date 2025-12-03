namespace TendalProject.Business.DTOs.Responses.Articulo
{
    public record DetalleArticuloResponse
    (Guid ArticuloId,
        string Codigo,
        string Nombre,
        string Descripcion,
        decimal Precio,
        int Stock,
        string NombreCategoria,
        string NombreProveedor,
        DateTime FechaRegistro);
}
