using TendalProject.Business.DTOs.Requests.Articulo;
using TendalProject.Business.DTOs.Responses.Articulo;
using TendalProject.Common.Results;

namespace TendalProject.Business.Interfaces
{
    public interface IArticuloService
    {
        Task<Result<Guid>> RegistrarArticuloAsync(RegistrarArticuloRequest request);
        Task<Result<Guid>> ActualizarArticuloAsync(ActualizarArticuloRequest request);
        Task<Result<string>> GenerarCodigoArticuloAsync();
        Task<Result<string>> ObtenerRutaImagenArticuloAsync(Guid articuloId);
        Task<Result<DetalleArticuloResponse>> DetalleArticuloAsync(Guid articuloId);
        Task<Result> ActualizarImagenArticuloAsync(ActualizarImagenArticuloRequest request);
        Task<Result<List<ListarArticulosResponse>>> ObtenerListaArticulosAsync();
        Task<Result<DatosActualizarArticuloResponse>> ObtenerArticuloActualizarAsync(Guid articuloId);
        Task<Result<Guid>> ModificarEstadoArticuloAsync(Guid articuloId);
    }
}
