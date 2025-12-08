using TendalProject.Entities.Entidades;

namespace TendalProject.Data.Interfaces
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Task<Usuario?> GetUsuarioConRolesPorEmailAsync(string email);
        Task<bool> ExisteEmailAsync(string email);
        Task<List<Rol>> GetRolesPorUsuarioIdAsync(Guid usuarioId);
        Task<Usuario?> GetUsuarioWithRolesAsync(Guid usuarioId);
    }
}
