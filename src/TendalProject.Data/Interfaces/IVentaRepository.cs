using TendalProject.Entities.Entidades;

namespace TendalProject.Data.Interfaces
{
    public interface IVentaRepository : IRepository<Venta>
    {
        Task<decimal> GetVentasPorClienteIdAsync(Guid clienteId);
        Task<int> GetCantidadVentasPorClienteIdAsync(Guid clienteId);
    }
}
