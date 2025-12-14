namespace TendalProject.Business.DTOs.Requests.Pedido
{
    public record CrearPedidoPendienteRequest
    (
        Guid ClienteId,
        DateTime FechaEntrega
    );
}
