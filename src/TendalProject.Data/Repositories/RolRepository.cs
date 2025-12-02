using TendalProject.Data.Context;
using TendalProject.Data.Interfaces;
using TendalProject.Entities.Entidades;

namespace TendalProject.Data.Repositories
{
    public class RolRepository : Repository<Rol>, IRolRepository
    {
        private readonly AppDbContext _appDbContext;
        public RolRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public Task<Guid> GetRolIdPorNombreAsync(string nombre)
        {
            var rolId = _appDbContext.TblRol
                .Where(r => r.Nombre == nombre)
                .Select(r => r.RolId)
                .FirstOrDefault();
            return Task.FromResult(rolId);
        }
    }
}
