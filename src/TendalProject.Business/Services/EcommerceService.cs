using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TendalProject.Business.DTOs.Requests.Ecommerce;
using TendalProject.Business.DTOs.Responses.Ecommerce;
using TendalProject.Business.Interfaces;
using TendalProject.Common.Results;
using TendalProject.Common.Time;
using TendalProject.Data.UnitOfWork;
using TendalProject.Entities.Entidades;
using TendalProject.Entities.Enum;

namespace TendalProject.Business.Services
{
    public class EcommerceService : IEcommerceService
    {
        private readonly IUnitOfWork _UoW;
        private readonly IDateTimeProvider _dateTimeProvider;

        public EcommerceService(
            IUnitOfWork uoW,
            IDateTimeProvider dateTimeProvider
            )
        {
            _UoW = uoW;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Result<Guid>> ActualizarCantidadItemCarritoAsync(ActualizarCantidadItenRequest request)
        {
            if (request.Cantidad <= 0)
                return Result<Guid>.Failure(Error.Validation("La cantidad debe ser mayor a 0."));

            var carrito = await _UoW.CarritoRepository.GetCarritoByClienteIdAsync(request.ClienteId);
            if (carrito is null)
                return Result<Guid>.Failure(Error.NotFound("Carrito no encontrado."));

            var itemSeleccionado = carrito.Items.FirstOrDefault(i => i.ItemId == request.ItemId);
            if (itemSeleccionado is null)
                return Result<Guid>.Failure(Error.NotFound("Item no encontrado en el carrito."));

            itemSeleccionado.Cantidad = request.Cantidad;

            await _UoW.SaveChangesAsync();
            return Result<Guid>.Success(request.ClienteId);
        }


        public async Task<Result<Guid>> AgregarItemAlCarritoAsync(SeleccionarArticuloRequest request)
        {
            await _UoW.BeginTransactionAsync();
            try
            {
                var articulo = await _UoW.ArticuloRepository.GetByIdAsync(request.ArticuloId);
                if (articulo is null)
                {
                    return Result<Guid>.Failure(Error.NotFound("Artículo no encontrado"));
                }

                var clienteId = Guid.Parse(request.ClienteId);
                var carrito = await _UoW.CarritoRepository.GetCarritoByClienteIdAsync(clienteId);

                if (carrito is null)
                {
                    carrito = new Carrito()
                    {
                        CarritoId = Guid.NewGuid(),
                        ClienteId = clienteId,
                        FechaCreacion = _dateTimeProvider.GetDateTimeNow(),
                        Items = new List<Item>()
                    };

                    await _UoW.CarritoRepository.AddAsync(carrito);
                    await _UoW.SaveChangesAsync();
                }
                if (carrito.Items == null)
                {
                    carrito.Items = new List<Item>();
                }
                var itemExistente = carrito.Items.FirstOrDefault(x => x.ArticuloId == articulo.ArticuloId);

                if (itemExistente is not null)
                {
                    itemExistente.Cantidad += request.Cantidad;
                }
                else
                {
                    var nuevoItem = new Item()
                    {
                        ItemId = Guid.NewGuid(),
                        CarritoId = carrito.CarritoId,
                        ArticuloId = articulo.ArticuloId,
                        Cantidad = request.Cantidad,
                        PrecioFinal = request.PrecioFinal
                    };

                    await _UoW.ItemRepository.AddAsync(nuevoItem);
                }

                await _UoW.SaveChangesAsync();
                await _UoW.CommitTransactionAsync();
                return Result<Guid>.Success(clienteId);
            }
            catch (Exception ex)
            {
                await _UoW.RollBackAsync();
                return Result<Guid>.Failure(Error.Internal("Error al agregar item al carrito: " + ex.Message));
            }
        }

        public async Task<Result<Guid>> EliminarItemCarritoAsync(EliminarItemCarritoRequest request)
        {
            var carrito = await _UoW.CarritoRepository.GetCarritoByClienteIdAsync(request.ClienteId);
            if(carrito is null)
            {
                return Result<Guid>.Failure(Error.NotFound());
            }
            var item = carrito.Items.FirstOrDefault(x => x.ItemId == request.ItemId);
            if(item is null)
            {
                return Result<Guid>.Failure(Error.NotFound());
            }
            _UoW.ItemRepository.EliminarItem(item);
            await _UoW.SaveChangesAsync();
            return Result<Guid>.Success(request.ClienteId);
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

        public async Task<Result<CarritoResponse>> ObtenerCarritoAsync(Guid clienteId)
        {
            var cliente = await _UoW.ClienteRepository.GetByIdAsync(clienteId);
            if (cliente is null || clienteId == Guid.Empty)
            {
                var carritoVacio = new CarritoResponse(Guid.Empty, clienteId, new List<ItemCarritoResponse>());
                return Result<CarritoResponse>.Success(carritoVacio);
            }
            var carrito = await _UoW.CarritoRepository.GetCarritoByClienteIdAsync(clienteId);
            if (carrito is null)
            {
                var carritoVacio = new CarritoResponse(Guid.Empty, clienteId, new List<ItemCarritoResponse>());
                return Result<CarritoResponse>.Success(carritoVacio);
            }
            var items = carrito.Items.Select(i => new ItemCarritoResponse(
                i.ItemId,
                i.ArticuloId,
                i.Articulo.Nombre,
                i.Articulo.Imagen,
                i.PrecioFinal,
                i.Cantidad,
                i.Cantidad * i.PrecioFinal
            )).ToList();

            var response = new CarritoResponse(
                carrito.CarritoId,
                clienteId,
                items
            );

            return Result<CarritoResponse>.Success(response);
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
