using TendalProject.Entities.Entidades;

namespace TendalProject.Data.Interfaces
{
    public interface IRolRepository : IRepository<Rol>
    {
        Task<Guid> GetRolIdPorNombreAsync(string nombre);
    }
}
