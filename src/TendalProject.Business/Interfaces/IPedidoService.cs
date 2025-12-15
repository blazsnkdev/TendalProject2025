using TendalProject.Business.DTOs.Requests.Pedido;
using TendalProject.Business.DTOs.Responses.Pedido;
using TendalProject.Common.Results;
using TendalProject.Entities.Entidades;
using TendalProject.Entities.Enum;

namespace TendalProject.Business.Interfaces
{
    public interface IPedidoService
    {
        Task<Result<List<ListaPedidosResponse>>> ObtenerPedidosAsync(
                    DateTime? fechaInicio = null,
                    DateTime? fechaFin = null,
                    string? codigo = null,
                    bool? proximosAEntregar = null,
                    string? ordenarPor = "fecha",
                    string? orden = "desc",
                    EstadoPedido? estado = null
                );
        Task<Result<List<HistorialPedidosClienteResponse>>> ObtenerPedidosPorClienteAsync(Guid clienteId);
        Task<Result<string>> GenerarCodigoPedidoAsync();
        Task<Result<Pedido>> CrearPedidoPendienteAsync(CrearPedidoPendienteRequest request);
        Task<Result<List<DetallePedidoResponse>>> ObtenerDetallesPedidoAsync(Guid pedidoId);
        Task<Result<Guid>> EnviarPedidoAsync(Guid pedidoId);
    }
}
