using TendalProject.Data.Interfaces;

namespace TendalProject.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        public IUsuarioRepository UsuarioRepository { get; }
        public IClienteRepository ClienteRepository { get; }
        public ICategoriaRepository CategoriaRepository { get; }
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollBackAsync();
    }
}
