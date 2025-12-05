using TendalProject.Business.DTOs.Requests.Proveedor;
using TendalProject.Business.DTOs.Responses.Proveedor;
using TendalProject.Common.Results;

namespace TendalProject.Business.Interfaces
{
    public interface IProveedorService
    {
        Task<Result<Guid>> RegistrarProveedorAsync(RegistrarProveedorRequest request);
        Task<Result> ActualizarProveedorAsync(ActualizarProveedorRequest request);
        Task<Result<ProveedorResponse>> DetalleProveedorAsync(Guid proveedorId);
        Task<Result<List<ProveedorResponse>>> ObtenerProveedoresAsync();
        Task<Result<Guid>> ModificarEstadoProveedorAsync(Guid proveedorId);
        Task<Result<List<ProveedorSelectListResponse>>> ObtenerProveedoresActivosSelectListAsync();
    }
}
