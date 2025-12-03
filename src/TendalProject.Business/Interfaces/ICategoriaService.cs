using TendalProject.Business.DTOs.Requests.Categoria;
using TendalProject.Business.DTOs.Responses.Categoria;
using TendalProject.Common.Results;

namespace TendalProject.Business.Interfaces
{
    public interface ICategoriaService
    {
        Task<Result<Guid>> RegistrarCategoriaAsync(RegistrarCategoriaRequest request);
        Task<Result<Guid>> ActualizarCategoriaAsync(ActualizarCategoriaRequest request);
        Task<Result> ModificarEstadoAsync(Guid categoriaId);
        Task<Result<CategoriaResponse>> ObtenerDetalleCategoriaAsync(Guid categoriaId);
        Task<Result<List<CategoriaResponse>>> ObtenerCategoriasAsync();
    }
}
