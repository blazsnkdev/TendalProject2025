using TendalProject.Business.DTOs.Requests.Articulo;
using TendalProject.Common.Results;

namespace TendalProject.Business.Interfaces
{
    public interface IArticuloService
    {
        Task<Result<Guid>> RegistrarArticuloAsync(RegistrarArticuloRequest request);
        Task<Result<string>> GenerarCodigoArticuloAsync();
    }
}
