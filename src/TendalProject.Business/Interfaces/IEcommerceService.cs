using TendalProject.Business.DTOs.Requests.Ecommerce;
using TendalProject.Business.DTOs.Responses.Ecommerce;
using TendalProject.Common.Results;

namespace TendalProject.Business.Interfaces
{
    public interface IEcommerceService
    {
        Task<Result<List<CatalogoArticulosResponse>>> ObtenerCatalogoFiltradoAsync(
                    Guid? categoriaId,
                    string? q,
                    decimal? minPrecio,
                    decimal? maxPrecio,
                    bool? oferta,
                    bool? disponible,
                    double? minRating,
                    bool? nuevo,
                    string? orden
                );
        Task<Result<DetalleArticuloSeleccionadoResponse>> ObtenerArticuloSelccionadoAsync(Guid articuloId);
        Task<Result<Guid>> AgregarItemAlCarritoAsync(SeleccionarArticuloRequest request);
        Task<Result<CarritoResponse>> ObtenerCarritoAsync(Guid clienteId);

    }
}
