namespace TendalProject.Business.DTOs.Responses.Pedido
{
    public record ItemsPedidoClienteResponse
    (
        Guid PedidoId,
        Guid ArticuloId,
        string CodigoArticulo,
        string Nombre,
        string Descripcion,
        int Cantidad,
        decimal Precio
    );
}
