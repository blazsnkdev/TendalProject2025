namespace TendalProject.Business.DTOs.Responses.Pedido
{
    public record ListaPedidosResponse
    (
        Guid PedidoId,
        string Codigo,
        string NombreCliente,
        DateTime FechaRegistro,
        DateTime? FechaEntrega,
        decimal Total,
        string Estado,
        int CantidadArticulos
    );
}
