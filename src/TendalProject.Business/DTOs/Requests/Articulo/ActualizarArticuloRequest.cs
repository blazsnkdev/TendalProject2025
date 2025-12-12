namespace TendalProject.Business.DTOs.Requests.Articulo
{
    public record ActualizarArticuloRequest
        (
            Guid ArticuloId,
            string Nombre,
            string Descripcion,
            decimal Precio,
            decimal? PrecioOferta,
            int Stock,
            bool Destacado,
            Guid CategoriaId,
            Guid ProveedorId
        );
}
