using Microsoft.EntityFrameworkCore;
using TendalProject.Data.Context;
using TendalProject.Data.Interfaces;
using TendalProject.Entities.Entidades;

namespace TendalProject.Data.Repositories
{
    public class DetallePedidoRepository : Repository<DetallePedido>, IDetallPedidoRepository
    {
        private readonly AppDbContext _appDbContext;
        public DetallePedidoRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<List<DetallePedido>> GetDetallesIncludArticuloByPedidoId(Guid pedidoId)
        {
            return await _appDbContext.TblDetallePedido
                .Where(dp=>dp.PedidoId == pedidoId)
                .Include(dp=>dp.Articulo)
                .ThenInclude(a=>a.Categoria)
                .ToListAsync();
        }
    }
}
