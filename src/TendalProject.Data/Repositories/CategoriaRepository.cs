using Microsoft.EntityFrameworkCore;
using TendalProject.Data.Context;
using TendalProject.Data.Interfaces;
using TendalProject.Entities.Entidades;
using TendalProject.Entities.Enum;

namespace TendalProject.Data.Repositories
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        private readonly AppDbContext _appDbContext;
        public CategoriaRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public Task<bool> ExisteNombreAsync(string nombre)
        {
            return _appDbContext.TblCategoria
                .AnyAsync(c => c.Nombre.ToLower() == nombre.ToLower());
        }

        public async Task<List<Categoria>> GetAllCategoriasActivasAsync()
        {
            return await _appDbContext.TblCategoria
                .Where(c => c.Estado == EstadoCategoria.Activo)
                .ToListAsync();
        }
    }
}
