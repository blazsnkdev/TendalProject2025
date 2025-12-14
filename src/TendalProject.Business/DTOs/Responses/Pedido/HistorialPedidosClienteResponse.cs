namespace TendalProject.Business.DTOs.Responses.Pedido
{
    public record HistorialPedidosClienteResponse
    (
        Guid PedidoId,
        string Codigo,
        decimal MontoTotal,
        DateTime FechaRegistro,
        DateOnly? FechaEntrega,
        string Estado,
        int CantidadArticulos
    );
}
