namespace TendalProject.Business.DTOs.Responses.Ecommerce
{
    public record ItemCarritoResponse
    (
        Guid ItemId,
        Guid ArticuloId,
        string NombreArticulo,
        string Imagen,
        decimal Precio,
        int Cantidad,
        decimal SubTotal
    );
}
