using Microsoft.EntityFrameworkCore;
using TendalProject.Data.Context;
using TendalProject.Data.Interfaces;
using TendalProject.Entities.Entidades;

namespace TendalProject.Data.Repositories
{
    public class CarritoRepository : Repository<Carrito>, ICarritoRepository
    {
        private readonly AppDbContext _appDbContext;
        public CarritoRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<Carrito?> GetCarritoByClienteIdAsync(Guid clienteId)
        {
            return await _appDbContext.TblCarrito
                .Include(c => c.Items)
                    .ThenInclude(i => i.Articulo).
                    Include(c=>c.Cliente)
                .FirstOrDefaultAsync(c => c.ClienteId == clienteId);
        }

    }
}
