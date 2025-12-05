using TendalProject.Entities.Entidades;

namespace TendalProject.Data.Interfaces
{
    public interface IArticuloRepository : IRepository<Articulo>
    {
        Task<Articulo?> GetArticuloWithIncludesByIdAsync(Guid articuloId);
        Task<List<Articulo>> GetArticulosWithCategoriaAsync();
        Task UpdateEstadoArticuloByIdAsync(Guid articuloId);
    }
}
