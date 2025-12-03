using TendalProject.Entities.Entidades;

namespace TendalProject.Data.Interfaces
{
    public interface IProveedorRepository : IRepository<Proveedor>
    {
        Task<bool> ExisteRucAsync(string ruc);
        Task<bool> ExisteEmailAsync(string email);
        Task<bool> ExisteTelefonoAsync(string telefono);
        Task<List<Proveedor>> GetProveedoresActivosAsync();
    }
}
