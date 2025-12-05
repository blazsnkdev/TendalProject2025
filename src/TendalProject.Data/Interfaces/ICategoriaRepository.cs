using TendalProject.Entities.Entidades;

namespace TendalProject.Data.Interfaces
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        Task<bool> ExisteNombreAsync(string nombre);
        Task<List<Categoria>> GetAllCategoriasAsync();
    }
}
