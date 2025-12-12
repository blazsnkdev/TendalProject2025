namespace TendalProject.Business.DTOs.Responses.Ecommerce
{
    public record CatalogoArticulosResponse
    (
        Guid ArticuloId,
        string NombreArticulo,
        string Descripcion,
        string NombreCategoria,
        decimal PrecioUnitario,
        int Stock,
        decimal? PrecioOferta,
        string Imagen,
        bool Disponible,
        double? Calificacion,
        bool EsNuevo
    );
}
