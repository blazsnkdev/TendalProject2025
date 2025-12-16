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
                .Where(v => v.Pedido.ClienteId == clienteId)
                .CountAsync();
        }

        public async Task<decimal> GetVentasHoyAsync()
        {
            var hoy = DateTime.Today;
            var manana = hoy.AddDays(1);
            return await _appDbContext.TblVenta
                .Where(x=>x.FechaVenta >= hoy && x.FechaVenta < manana)
                .SumAsync(x => x.Pedido.Total);
        }

        public async Task<decimal> GetVentasMesAsync()
        {
            var hoy = DateTime.Now;
            var inicioMes = new DateTime(hoy.Year, hoy.Month, 1);
            var inicioMesSiguiente = inicioMes.AddMonths(1);

            return await _appDbContext.TblVenta
                .Where(x => x.FechaVenta >= inicioMes &&
                            x.FechaVenta < inicioMesSiguiente)
                .SumAsync(x => x.Pedido.Total);
        }

        public async Task<decimal> GetVentasPorClienteIdAsync(Guid clienteId)
        {
            var totalVentas = await _appDbContext.TblVenta
                .Where(v => v.Pedido.ClienteId == clienteId)
                .SumAsync(v => v.Pedido.Total);
            return totalVentas;
        }
    }
}
