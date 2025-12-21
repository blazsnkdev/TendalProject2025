using TendalProject.Entities.Entidades;

namespace TendalProject.Data.Interfaces
{
    public interface IPedidoRepository : IRepository<Pedido>
    {
        Task<List<Pedido>> GetPedidosPorClienteAsync(Guid clienteId);
        Task<List<Pedido>> GetPedidosIncludsPorClienteAsync(Guid clienteId);
        IQueryable<Pedido> GetPedidosIncludsAsync();
        Task<Pedido?> GetPedidoPendienteByClienteIdAsync(Guid clienteId);
        Task<Pedido?> GetPedidoIncludsByIdAsync(Guid pedidoId);
        Task<int> GetPedidosHoyAsync();
        Task<int> GetPedidosPendientesAsync();
    }
}
