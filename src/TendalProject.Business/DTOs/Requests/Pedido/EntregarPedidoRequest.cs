using TendalProject.Entities.Enum;

namespace TendalProject.Business.DTOs.Requests.Pedido
{
    public record EntregarPedidoRequest
    (
        Guid PedidoId,
        string EstadoActual
        );
}
