using TendalProject.Entities.Entidades;

namespace TendalProject.Data.Interfaces
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Task<Usuario?> GetUsuarioConRolesPorEmailAsync(string email);
    }
}
