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
    }
}
