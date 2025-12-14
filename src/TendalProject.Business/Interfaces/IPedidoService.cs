using TendalProject.Business.DTOs.Responses.Pedido;
using TendalProject.Common.Results;

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
                    string? orden = "desc"
                );
        Task<Result<List<HistorialPedidosClienteResponse>>> ObtenerPedidosPorClienteAsync(Guid clienteId);
    }
}
