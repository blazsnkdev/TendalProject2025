namespace TendalProject.Business.DTOs.Responses.Articulo
{
    public record DatosActualizarArticuloResponse
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
