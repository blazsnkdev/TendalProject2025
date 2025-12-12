namespace TendalProject.Business.DTOs.Responses.Ecommerce
{
    public record DetalleArticuloSeleccionadoResponse
    (
        Guid ArticuloId,
        string NombreArticulo,
        string Descripcion,
        string NombreCategoria,
        decimal? PrecioFinal,
        int Stock,
        string Imagen,
        int Calificacion
    );
}
