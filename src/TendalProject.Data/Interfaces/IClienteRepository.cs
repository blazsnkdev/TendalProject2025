using TendalProject.Entities.Entidades;

namespace TendalProject.Data.Interfaces
{
    public interface IClienteRepository : IRepository<Cliente>
    {
        Task<bool> ExisteClienteConEmailAsync(string email);
        Task<List<Cliente>> GetAllClientesByNombre(string nombre);
    }
}
