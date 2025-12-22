using Microsoft.EntityFrameworkCore.Storage;
using TendalProject.Data.Context;
using TendalProject.Data.Interfaces;
using TendalProject.Data.Repositories;

namespace TendalProject.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly AppDbContext _appDbContext;
        private IDbContextTransaction? _transaction;

        public IUsuarioRepository UsuarioRepository { get; }
        public IClienteRepository ClienteRepository { get; }
        public ICategoriaRepository CategoriaRepository { get; }
        public IRolRepository RolRepository { get; }
        public IProveedorRepository ProveedorRepository { get; }
        public IArticuloRepository ArticuloRepository { get; }
        public IPedidoRepository PedidoRepository { get; }
        public IVentaRepository VentaRepository { get; }
        public ICarritoRepository CarritoRepository { get; }
        public IItemRepository ItemRepository { get; }
        public IDetallPedidoRepository DetallePedidoRepository { get; }
        public IReseñaRepository ReseñaRepository { get; }

        public UnitOfWork(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            UsuarioRepository = new UsuarioRepository(_appDbContext);
            ClienteRepository = new ClienteRepository(_appDbContext);
            CategoriaRepository = new CategoriaRepository(_appDbContext);
            RolRepository = new RolRepository(_appDbContext);
            ProveedorRepository = new ProveedorRepository(_appDbContext);
            ArticuloRepository = new ArticuloRepository(_appDbContext);
            PedidoRepository = new PedidoRepository(_appDbContext);
            VentaRepository = new VentaRepository(_appDbContext);
            CarritoRepository = new CarritoRepository(_appDbContext);
            ItemRepository = new ItemRepository(_appDbContext);
            DetallePedidoRepository = new DetallePedidoRepository(_appDbContext);
            ReseñaRepository = new ReseñaRepository(_appDbContext);
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

        public async Task RollBackAsync()
        {
            if (_transaction != null)
                await _transaction.RollbackAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _appDbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _appDbContext.Dispose();
        }
    }
}
