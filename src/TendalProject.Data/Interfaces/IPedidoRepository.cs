using TendalProject.Entities.Entidades;
using TendalProject.Entities.Enum;

namespace TendalProject.Data.Interfaces
{
    public interface IPedidoRepository : IRepository<Pedido>
    {
        Task<List<Pedido>> GetPedidosPorClienteAsync(Guid clienteId);
        Task<List<Pedido>> GetPedidosIncludsPorClienteAsync(Guid clienteId);
    }
}
