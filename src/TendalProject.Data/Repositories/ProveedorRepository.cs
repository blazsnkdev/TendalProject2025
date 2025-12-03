using Microsoft.EntityFrameworkCore;
using TendalProject.Data.Context;
using TendalProject.Data.Interfaces;
using TendalProject.Entities.Entidades;
using TendalProject.Entities.Enum;

namespace TendalProject.Data.Repositories
{
    public class ProveedorRepository : Repository<Proveedor>, IProveedorRepository
    {
        private readonly AppDbContext _appDbContext;
        public ProveedorRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> ExisteEmailAsync(string email)
        {
            return await _appDbContext.TblProveedor.AnyAsync(p => p.Email == email);
        }

        public async Task<bool> ExisteRucAsync(string ruc)
        {
            return await _appDbContext.TblProveedor.AnyAsync(p => p.Ruc == ruc);
        }

        public async Task<bool> ExisteTelefonoAsync(string telefono)
        {
            return await _appDbContext.TblProveedor.AnyAsync(p => p.Telefono == telefono);
        }

        public Task<List<Proveedor>> GetProveedoresActivosAsync()
        {
            return _appDbContext.TblProveedor
                .Where(p => p.Estado == EstadoProveedor.Activo)
                .ToListAsync();
        }
    }
}
