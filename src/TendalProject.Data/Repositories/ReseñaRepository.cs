using TendalProject.Data.Context;
using TendalProject.Data.Interfaces;
using TendalProject.Entities.Entidades;

namespace TendalProject.Data.Repositories
{
    public class ReseñaRepository : Repository<Reseña>, IReseñaRepository
    {
        public ReseñaRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
