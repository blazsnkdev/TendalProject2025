
using Microsoft.EntityFrameworkCore.Storage;
using TendalProject.Data.Context;
using TendalProject.Data.Interfaces;

namespace TendalProject.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _appDbContext;
        private IDbContextTransaction? _transaction;

        public IUsuarioRepository UsuarioRepository { get; }
        public IClienteRepository ClienteRepository { get; }
        public ICategoriaRepository CategoriaRepository { get; }
        public IRolRepository RolRepository { get; }

        public UnitOfWork(
            AppDbContext appDbContext,
            IUsuarioRepository usuarioRepository,
            IClienteRepository clienteRepository,
            ICategoriaRepository categoriaRepository,
            IRolRepository rolRepository
            )
        {
            _appDbContext = appDbContext;
            UsuarioRepository = usuarioRepository;
            ClienteRepository = clienteRepository;
            CategoriaRepository = categoriaRepository;
            RolRepository = rolRepository;
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _appDbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _appDbContext.SaveChangesAsync();
            if (_transaction != null)
                await _transaction.CommitAsync();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _appDbContext.Dispose();
        }

        public async Task RollBackAsync()
        {
            if (_transaction != null)
                await _transaction.RollbackAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _appDbContext.SaveChangesAsync();
        }
    }
}
