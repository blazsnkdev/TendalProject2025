namespace TendalProject.Data.Interfaces
{
    public interface ICarritoRepository : IRepository<Carrito>
    {
        Task<Carrito?> GetCarritoByClienteIdAsync(Guid clienteId);
    }
}
