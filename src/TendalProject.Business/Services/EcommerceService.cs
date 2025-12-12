using TendalProject.Business.DTOs.Responses.Ecommerce;
using TendalProject.Business.Interfaces;
using TendalProject.Common.Results;
using TendalProject.Common.Time;
using TendalProject.Data.UnitOfWork;
using TendalProject.Entities.Enum;

namespace TendalProject.Business.Services
{
    public class EcommerceService : IEcommerceService
    {
        private readonly IUnitOfWork _UoW;
        private readonly IDateTimeProvider _dateTimeProvider;

        public EcommerceService(
            IUnitOfWork uoW,
            IDateTimeProvider dateTimeProvider)
        {
            _UoW = uoW;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Result<DetalleArticuloSeleccionadoResponse>> ObtenerArticuloSelccionadoAsync(Guid articuloId)
        {
            if(articuloId == Guid.Empty)
            {
                return Result<DetalleArticuloSeleccionadoResponse>.Failure(Error.Validation("El id es invalido"));
            }
            var articulo = await _UoW.ArticuloRepository.GetArticuloWithIncludesByIdAsync(articuloId);
            if(articulo is null)
            {
                return Result<DetalleArticuloSeleccionadoResponse>.Failure(Error.NotFound("Articulo no encontrado"));
            }
            var precioFinal = articulo.PrecioOferta is 0 ? articulo.Precio : articulo.PrecioOferta;
            var response = new DetalleArticuloSeleccionadoResponse(
                articulo.ArticuloId,
                articulo.Nombre,
                articulo.Descripcion,
                articulo.Categoria!.Nombre,
                precioFinal,
                articulo.Stock,
                articulo.Imagen,
                articulo.Reseñas.Any()
                    ? (int)Math.Round(articulo.Reseñas.Average(r => r.Puntuacion), MidpointRounding.AwayFromZero)
                    : 0
                );
            return Result<DetalleArticuloSeleccionadoResponse>.Success(response);
        }

        public async Task<Result<List<CatalogoArticulosResponse>>>ObtenerCatalogoFiltradoAsync(
            Guid? categoriaId,
            string? q,
            decimal? minPrecio,
            decimal? maxPrecio,
            bool? oferta,
            bool? disponible,
            double? minRating,
            bool? nuevo,
            string? orden
        )
        {
            var articulos = await _UoW.ArticuloRepository
                .GetArticulosDisponiblesWithCategoriaAsync();

            var ahora = _dateTimeProvider.GetDateTimeNow();

            if (categoriaId.HasValue)
                articulos = articulos.Where(a => a.CategoriaId == categoriaId).ToList();

            if (!string.IsNullOrWhiteSpace(q))
                articulos = articulos.Where(a =>
                    a.Nombre.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    a.Descripcion.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    a.Categoria!.Nombre.Contains(q, StringComparison.OrdinalIgnoreCase)
                ).ToList();

            if (minPrecio.HasValue)
                articulos = articulos.Where(a => a.Precio >= minPrecio).ToList();

            if (maxPrecio.HasValue)
                articulos = articulos.Where(a => a.Precio <= maxPrecio).ToList();

            if (oferta == true)
                articulos = articulos.Where(a => a.PrecioOferta.HasValue).ToList();

            if (disponible == true)
                articulos = articulos.Where(a => a.Stock > 0).ToList();

            if (minRating.HasValue)
                articulos = articulos.Where(a =>
                    a.Reseñas.Any() &&
                    a.Reseñas.Average(r => r.Puntuacion) >= minRating
                ).ToList();

            if (nuevo == true)
                articulos = articulos.Where(a =>
                    (ahora - a.FechaRegistro).TotalDays <= 5
                ).ToList();

            articulos = orden switch
            {
                "precio_asc" => articulos.OrderBy(a => a.Precio).ToList(),
                "precio_desc" => articulos.OrderByDescending(a => a.Precio).ToList(),
                "nuevo" => articulos.OrderByDescending(a => a.FechaRegistro).ToList(),
                _ => articulos.OrderBy(a => a.Nombre).ToList()
            };

            var response = articulos.Select(a =>
                new CatalogoArticulosResponse(
                    a.ArticuloId,
                    a.Nombre,
                    a.Descripcion,
                    a.Categoria!.Nombre,
                    a.Precio,
                    a.Stock,
                    a.PrecioOferta,
                    a.Imagen,
                    a.Estado == EstadoArticulo.Activo,
                    a.Reseñas.Any() ? a.Reseñas.Average(r => r.Puntuacion) : 0,
                    (ahora - a.FechaRegistro).TotalDays <= 5
                )
            ).ToList();

            return Result<List<CatalogoArticulosResponse>>.Success(response);
        }

    }
}
