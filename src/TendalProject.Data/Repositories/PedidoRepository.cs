using Microsoft.EntityFrameworkCore;
using TendalProject.Data.Context;
using TendalProject.Data.Interfaces;
using TendalProject.Entities.Entidades;
using TendalProject.Entities.Enum;

namespace TendalProject.Data.Repositories
{
    public class PedidoRepository : Repository<Pedido>, IPedidoRepository
    {
        private readonly AppDbContext _appDbContext;
        public PedidoRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Pedido?> GetPedidoIncludsByIdAsync(Guid pedidoId)
        {
            return await _appDbContext.TblPedido
                .Where(p => p.PedidoId == pedidoId)
                .Include(c => c.Cliente)
                .Include(d => d.Detalles)
                .FirstOrDefaultAsync();
        }

        public async Task<Pedido?> GetPedidoPendienteByClienteIdAsync(Guid clienteId)
        {
            return await _appDbContext.TblPedido
                .Include(p => p.Detalles)
                .FirstOrDefaultAsync(p => p.ClienteId == clienteId && p.Estado == EstadoPedido.Pendiente);
        }

        public async Task<int> GetPedidosHoyAsync()
        {
            var hoy = DateTime.Today;
            var manana = hoy.AddDays(1);
            return await _appDbContext.TblPedido
                .Where(p=>p.FechaRegistro >= hoy && p.FechaRegistro < manana)
                .CountAsync();
        }

        public IQueryable<Pedido> GetPedidosIncludsAsync()
        {
            return _appDbContext.TblPedido
                .Include(p => p.Cliente)
                .Include(p => p.Detalles)
                .ThenInclude(ip => ip.Articulo)
                .AsQueryable();
        }

        public Task<List<Pedido>> GetPedidosIncludsPorClienteAsync(Guid clienteId)//NOTE: esto no se esta usando
        {
            return _appDbContext.TblPedido
                .Include(p => p.Detalles)
                .ThenInclude(ip => ip.Articulo)
                .Where(p => p.ClienteId == clienteId)
                .ToListAsync();
        }

        public async Task<int> GetPedidosPendientesAsync()
        {
            return await _appDbContext.TblPedido.Where(p => p.Estado == EstadoPedido.Pendiente).CountAsync();
        }

        public async Task<List<Pedido>> GetPedidosPorClienteAsync(Guid clienteId)
        {
            return await _appDbContext.TblPedido
                .Where(p => p.ClienteId == clienteId)
                .ToListAsync();
        }

    }
}
