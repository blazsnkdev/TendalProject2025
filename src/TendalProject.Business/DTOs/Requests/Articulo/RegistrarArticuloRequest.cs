namespace TendalProject.Business.DTOs.Requests.Articulo
{
    public record RegistrarArticuloRequest
    (
        Guid ArticuloId,
        string Nombre,
        string Descripcion,
        decimal Precio,
        int Stock,
        Guid CategoriaId,
        Guid ProveedorId,
        string Imagen
    );
}
