using TendalProject.Business.DTOs.Responses.Cliente;
using TendalProject.Common.Results;

namespace TendalProject.Business.Interfaces
{
    public interface IClienteService
    {
        Task<Result<List<ListarClienteResponse>>> ListarClientesAsync(string? nombre);
        Task<Result<DetalleClienteResponse>> ObtenerDetalleClienteAsync(Guid clienteId);
    }
}
