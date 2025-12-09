using Microsoft.EntityFrameworkCore;
using TendalProject.Data.Context;
using TendalProject.Data.Interfaces;
using TendalProject.Entities.Entidades;

namespace TendalProject.Data.Repositories
{
    public class VentaRepository : Repository<Venta>, IVentaRepository
    {
        private readonly AppDbContext _appDbContext;
        public VentaRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public Task<int> GetCantidadVentasPorClienteIdAsync(Guid clienteId)
        {
            return _appDbContext.TblVenta
                .Where(v => v.ClienteId == clienteId)
                .CountAsync();
        }

        public async Task<decimal> GetVentasPorClienteIdAsync(Guid clienteId)
        {
            var totalVentas = await _appDbContext.TblVenta
                .Where(v => v.ClienteId == clienteId)
                .SumAsync(v => v.ImporteTotal);
            return totalVentas;
        }
    }
}
