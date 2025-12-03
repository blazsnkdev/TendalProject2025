using Microsoft.EntityFrameworkCore;
using TendalProject.Data.Context;
using TendalProject.Data.Interfaces;
using TendalProject.Entities.Entidades;

namespace TendalProject.Data.Repositories
{
    public class ArticuloRepository : Repository<Articulo>, IArticuloRepository
    {
        private readonly AppDbContext _appDbContext;
        public ArticuloRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Articulo?> GetArticuloWithIncludesByIdAsync(Guid articuloId)
        {
            return await _appDbContext.TblArticulo
                .AsNoTracking()
                .Where(a => a.ArticuloId == articuloId)
                .Include(c => c.Categoria)
                .Include(p => p.Proveedor)
                .FirstOrDefaultAsync();
        }
    }
}
