using Microsoft.EntityFrameworkCore;
using TendalProject.Data.Context;
using TendalProject.Data.Interfaces;
using TendalProject.Entities.Entidades;

namespace TendalProject.Data.Repositories
{
    public class ClienteRepository : Repository<Cliente>, IClienteRepository
    {
        private readonly AppDbContext _appDbContext;
        public ClienteRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<bool> ExisteClienteConEmailAsync(string email)
        {
            return await _appDbContext.TblCliente.AnyAsync(c => c.CorreoElectronico == email);
        }

        public async Task<List<Cliente>> GetAllClientesByNombre(string nombre)
        {
            return await _appDbContext.TblCliente.Where(c => c.Nombre.Contains(nombre)).ToListAsync();
        }
    }
}
