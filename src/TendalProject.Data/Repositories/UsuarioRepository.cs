using Microsoft.EntityFrameworkCore;
using TendalProject.Data.Context;
using TendalProject.Data.Interfaces;
using TendalProject.Entities.Entidades;

namespace TendalProject.Data.Repositories
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        private readonly AppDbContext _appDbContext;
        public UsuarioRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<bool> ExisteEmailAsync(string email)
        {
            return await _appDbContext.TblUsuario
                .AnyAsync(u => u.Email == email);
        }

        public Task<List<Rol>> GetRolesPorUsuarioIdAsync(Guid usuarioId)
        {
            return _appDbContext.TblUsuarioRol
                .AsNoTracking()
                .Where(ur => ur.UsuarioId == usuarioId)
                .Select(ur => ur.Rol)
                .ToListAsync();
        }

        public async Task<Usuario?> GetUsuarioConRolesPorEmailAsync(string email)
        {
            return await _appDbContext.TblUsuario
                .AsNoTracking()
                .Include(u => u.UsuariosRoles)
                    .ThenInclude(ur => ur.Rol)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public Task<Usuario?> GetUsuarioWithRolesAsync(Guid usuarioId)
        {
            return _appDbContext.TblUsuario
                .AsNoTracking()
                .Include(u => u.UsuariosRoles)
                    .ThenInclude(ur => ur.Rol)
                .FirstOrDefaultAsync(u => u.UsuarioId == usuarioId);
        }

        public async Task<int> UpdateEstadoAsync(Guid usuarioId)
        {
            var estadoActual = await _appDbContext.TblUsuario
                .Where(u => u.UsuarioId == usuarioId)
                .Select(u => u.Activo)
                .FirstOrDefaultAsync();

            return await _appDbContext.TblUsuario
                .Where(u => u.UsuarioId == usuarioId)
                .ExecuteUpdateAsync(u =>
                    u.SetProperty(x => x.Activo, !estadoActual));
        }
    }
}
