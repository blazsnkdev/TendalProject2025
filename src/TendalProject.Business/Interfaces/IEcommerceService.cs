using TendalProject.Business.DTOs.Requests.Ecommerce;
using TendalProject.Business.DTOs.Responses.Ecommerce;
using TendalProject.Common.Results;

namespace TendalProject.Business.Interfaces
{
    public interface IEcommerceService
    {
        //TODO: refactorizar esto con un request
        Task<Result<List<CatalogoArticulosResponse>>> ObtenerCatalogoFiltradoAsync(Guid? categoriaId,string? q,decimal? minPrecio,decimal? maxPrecio,bool? oferta,bool? disponible,double? minRating,bool? nuevo,string? orden);
        Task<Result<DetalleArticuloSeleccionadoResponse>> ObtenerArticuloSelccionadoAsync(Guid articuloId);
        Task<Result<Guid>> AgregarItemAlCarritoAsync(SeleccionarArticuloRequest request);
        Task<Result<CarritoResponse>> ObtenerCarritoAsync(Guid clienteId);
        Task<Result<Guid>> ActualizarCantidadItemCarritoAsync(ActualizarCantidadItenRequest request);
        Task<Result<Guid>> EliminarItemCarritoAsync(EliminarItemCarritoRequest request);
        Task<Result<Guid>> VaciarItemsCarritoAsync(Guid clienteId);

    }
}
