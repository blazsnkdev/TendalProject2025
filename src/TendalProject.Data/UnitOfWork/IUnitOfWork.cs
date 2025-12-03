using TendalProject.Data.Interfaces;

namespace TendalProject.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        public IUsuarioRepository UsuarioRepository { get; }
        public IClienteRepository ClienteRepository { get; }
        public ICategoriaRepository CategoriaRepository { get; }
        public IRolRepository RolRepository { get; }
        public IProveedorRepository ProveedorRepository { get; }
        public IArticuloRepository ArticuloRepository { get;}
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollBackAsync();
    }
}
