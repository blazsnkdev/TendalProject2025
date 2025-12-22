namespace TendalProject.Business.DTOs.Responses.Pedido
{
    public record DetallePedidoClienteResponse
        (
            Guid PedidoId,
            Guid ClienteId,
            string CodigoPedido,
            DateTime FechaRegistro,
            string Estado,
            DateTime? FechaEntrega,
            DateTime? FechaEnvio,
            DateTime? FechaPago,
            decimal Igv,
            decimal SubTotal,
            decimal Total,
            int CantidadItems,
            List<ItemsPedidoClienteResponse> Items
        );
}
