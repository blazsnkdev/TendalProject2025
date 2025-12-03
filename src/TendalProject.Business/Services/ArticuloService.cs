using Microsoft.EntityFrameworkCore;
using TendalProject.Business.DTOs.Requests.Articulo;
using TendalProject.Business.DTOs.Responses.Articulo;
using TendalProject.Business.Interfaces;
using TendalProject.Common.Results;
using TendalProject.Common.Time;
using TendalProject.Common.Utils;
using TendalProject.Data.UnitOfWork;
using TendalProject.Entities.Entidades;
using TendalProject.Entities.Enum;

namespace TendalProject.Business.Services
{
    public class ArticuloService : IArticuloService
    {
        private readonly IUnitOfWork _UoW;
        private readonly IDateTimeProvider _dateTimeProvider;
        public ArticuloService(
            IUnitOfWork uoW,
            IDateTimeProvider dateTimeProvider
            )
        {
            _UoW = uoW;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Result<DetalleArticuloResponse>> DetalleArticuloAsync(Guid articuloId)
        {
            var articulo = await _UoW.ArticuloRepository.GetArticuloWithIncludesByIdAsync(articuloId);
            if(articulo is null)
            {
                return Result<DetalleArticuloResponse>.Failure(Error.NotFound("El articulo no existe"));
            }
            var response = new DetalleArticuloResponse(
                articulo.ArticuloId,
                articulo.Codigo,
                articulo.Nombre,
                articulo.Descripcion,
                articulo.Precio,
                articulo.Stock,
                articulo.Categoria!.Nombre,
                articulo.Proveedor!.Nombre,
                articulo.FechaRegistro);
            return Result<DetalleArticuloResponse>.Success(response);
        }

        public Task<Result<ActualizarImagenArticuloResponse>> DetalleImagenArticuloAsync(Guid articuloId)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<string>> GenerarCodigoArticuloAsync()
        {
            var pedidos = await _UoW.ArticuloRepository.GetAllAsync();
            if (pedidos is null || !pedidos.Any())
            {
                return Result<string>.Success("ART-00000001");
            }
            else
            {
                var ultimoArticulo = pedidos
                    .OrderByDescending(p => p.FechaRegistro)
                    .FirstOrDefault();
                if (ultimoArticulo is null)
                {
                    return Result<string>.Success("ART-00000001");
                }
                var ultimoCodigo = ultimoArticulo.Codigo;
                var numeroUltimoCodigo = int.Parse(ultimoCodigo.Split('-')[1]);
                var nuevoNumeroCodigo = numeroUltimoCodigo + 1;
                var nuevoCodigo = $"ART-{nuevoNumeroCodigo.ToString("D8")}";
                return Result<string>.Success(nuevoCodigo);
            }
        }

        public async Task<Result<string>> ObtenerRutaImagenArticuloAsync(Guid articuloId)
        {
            var articulo = await _UoW.ArticuloRepository.GetByIdAsync(articuloId);
            if (articulo is null)
            {
                return Result<string>.Failure(Error.NotFound("El articulo no existe."));
            }
            if (!StringUtils.IsNullOrWhiteSpace(articulo.Imagen))
            {
                return Result<string>.Success("img/Articulo/no-image.jpg");//NOTE: ojito mi bro esto tengo que agregar una img por defect
            }
            return Result<string>.Success(articulo.Imagen);
        }

        public async Task<Result<Guid>> RegistrarArticuloAsync(RegistrarArticuloRequest request)
        {
            try
            {
                if (!StringUtils.IsNullOrWhiteSpace(
                request.Nombre,
                request.Descripcion,
                request.Imagen))
                {
                    return Result<Guid>.Failure(Error.Validation("Los campos Nombre, Descripcion e Imagen son obligatorios."));
                }
                var codigo = await GenerarCodigoArticuloAsync();
                if (codigo.Value is null)
                {
                    return Result<Guid>.Failure(Error.Internal("No se pudo generar el codigo del articulo."));
                }
                var articulo = new Articulo
                {
                    ArticuloId = request.ArticuloId,
                    Codigo = codigo.Value,
                    Nombre = request.Nombre,
                    Descripcion = request.Descripcion,
                    Precio = request.Precio,
                    PrecioOferta = 0,
                    Stock = request.Stock,
                    CategoriaId = request.CategoriaId,
                    ProveedorId = request.ProveedorId,
                    Destacado = false,
                    Estado = EstadoArticulo.Activo,
                    FechaRegistro = _dateTimeProvider.GetDateTimeNow(),
                    Imagen = request.Imagen,
                    CantidadVentas = 0
                };
                await _UoW.ArticuloRepository.AddAsync(articulo);
                await _UoW.SaveChangesAsync();
                return Result<Guid>.Success(articulo.ArticuloId);
            }
            catch (DbUpdateException ex)
            {
                return Result<Guid>.Failure(Error.Database($"Error de base de datos al registrar el articulo: {ex.Message}"));
            }
            catch (Exception ex)
            {
                return Result<Guid>.Failure(Error.Internal($"Error al registrar el articulo: {ex.Message}"));
            }
        }
    }
}
