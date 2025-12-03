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
    }
}
