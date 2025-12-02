using Microsoft.EntityFrameworkCore;
using TendalProject.Data.Context;
using TendalProject.Data.Interfaces;
using TendalProject.Entities.Entidades;

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
    }
}
