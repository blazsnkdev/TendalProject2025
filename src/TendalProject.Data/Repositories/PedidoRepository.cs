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

        public async Task<List<Pedido>> GetPedidosPorClienteAsync(Guid clienteId)
        {
            return await _appDbContext.TblPedido
                .Where(p => p.ClienteId == clienteId)
                .ToListAsync();
        }

    }
}
