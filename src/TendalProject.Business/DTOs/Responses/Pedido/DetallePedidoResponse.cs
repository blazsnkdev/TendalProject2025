namespace TendalProject.Business.DTOs.Responses.Pedido
{
    public record DetallePedidoResponse
    (
        Guid DetallePedidoId,
        string NombreArticulo,
        string CodigoArticulo,
        string DescripcionArticulo,
        string NombreCategoria,
        int Cantidad,
        decimal PrecioFinal,
        decimal SubTotal
    );
}
