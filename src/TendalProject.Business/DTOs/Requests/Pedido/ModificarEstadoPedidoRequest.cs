using TendalProject.Entities.Enum;

namespace TendalProject.Business.DTOs.Requests.Pedido
{
    public record ModificarEstadoPedidoRequest
    (
        Guid PedidoId,
        EstadoPedido EstadoPedido
    );
}
