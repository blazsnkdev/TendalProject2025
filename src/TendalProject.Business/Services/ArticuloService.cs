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
        public async Task<Result<Guid>> ActualizarArticuloAsync(ActualizarArticuloRequest request)
        {
            try
            {
                if (request.ArticuloId == Guid.Empty)
                {
                    return Result<Guid>.Failure(Error.Validation("El campo ArticuloId es obligatorio."));
                }
                if(!StringUtils.IsNullOrWhiteSpace(request.Nombre,request.Descripcion))
                {
                    return Result<Guid>.Failure(Error.Validation("Los campos Nombre y Descripcion son obligatorios."));
                }
                var articulo = await _UoW.ArticuloRepository.GetByIdAsync(request.ArticuloId);
                if (articulo == null) { 
                    return Result<Guid>.Failure(Error.NotFound("Artículo no encontrado."));
                }
                articulo.Nombre = request.Nombre;
                articulo.Descripcion = request.Descripcion;
                articulo.Precio = request.Precio;
                articulo.PrecioOferta = request.PrecioOferta;
                articulo.Stock = request.Stock;
                articulo.Destacado = request.Destacado;
                articulo.CategoriaId = request.CategoriaId;
                articulo.ProveedorId = request.ProveedorId;
                _UoW.ArticuloRepository.Update(articulo);
                await _UoW.SaveChangesAsync();
                return Result<Guid>.Success(articulo.ArticuloId);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Result<Guid>.Failure(Error.Conflict("El artículo ha sido modificado por otro usuario. Por favor, vuelva a intentarlo."));
            }
            catch (DbUpdateException ex)
            {
                return Result<Guid>.Failure(Error.Database($"Error de base de datos al actualizar el articulo: {ex.Message}"));
            }
            catch (Exception ex)
            {
                return Result<Guid>.Failure(Error.Internal($"Error al actualizar el articulo: {ex.Message}"));
            }
        }

        public async Task<Result> ActualizarImagenArticuloAsync(ActualizarImagenArticuloRequest request)
        {
            var articulo = await _UoW.ArticuloRepository.GetByIdAsync(request.ArticuloId);
            if (articulo == null)
            {
                return Result<string>.Failure(Error.NotFound("Artículo no encontrado."));
            }
            var extension = Path.GetExtension(request.Imagen.FileName);
            var nombreArchivo = $"{articulo.ArticuloId}{extension}";
            var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Articulo");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            var rutaArchivo = Path.Combine(folder, nombreArchivo);
            using (var stream = new FileStream(rutaArchivo, FileMode.Create))
            {
                await request.Imagen.CopyToAsync(stream);
            }
            articulo.Imagen = $"img/Articulo/{nombreArchivo}";
            await _UoW.SaveChangesAsync();
            return Result.Success();
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
                articulo.FechaRegistro,
                articulo.Estado.ToString());
            return Result<DetalleArticuloResponse>.Success(response);
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

        public async Task<Result<Guid>> ModificarEstadoArticuloAsync(Guid articuloId)
        {
            if (articuloId == Guid.Empty)
            {
                return Result<Guid>.Failure(Error.Validation("El campo ArticuloId es obligatorio."));
            }
            var articulo = await _UoW.ArticuloRepository.GetByIdAsync(articuloId);
            if(articulo is null)
            {
                return Result<Guid>.Failure(Error.NotFound("El articulo no existe."));
            }
            articulo.Estado = articulo.Estado == EstadoArticulo.Activo ? EstadoArticulo.Inactivo : EstadoArticulo.Activo;
            await _UoW.SaveChangesAsync();
            return Result<Guid>.Success(articulo.ArticuloId);
        }

        public async Task<Result<DatosActualizarArticuloResponse>> ObtenerArticuloActualizarAsync(Guid articuloId)
        {
            if(articuloId == Guid.Empty)
            {
                return Result<DatosActualizarArticuloResponse>.Failure(Error.Validation("El campo ArticuloId es obligatorio."));
            }
            var articulo = await _UoW.ArticuloRepository.GetArticuloWithIncludesByIdAsync(articuloId);
            if(articulo is null)
            {
                return Result<DatosActualizarArticuloResponse>.Failure(Error.NotFound("El articulo no existe."));
            }
            var response = new DatosActualizarArticuloResponse(
                articulo.ArticuloId,
                articulo.Nombre,
                articulo.Descripcion,
                articulo.Precio,
                articulo.PrecioOferta,
                articulo.Stock,
                articulo.Destacado,
                articulo.CategoriaId ?? Guid.Empty,
                articulo.ProveedorId ?? Guid.Empty
            );
            return Result<DatosActualizarArticuloResponse>.Success(response);
        }

        public async Task<Result<List<ListarArticulosResponse>>> ObtenerListaArticulosAsync()
        {
            var articulos = await _UoW.ArticuloRepository.GetArticulosWithCategoriaAsync();
            if (articulos is null || !articulos.Any())
            {
                return Result<List<ListarArticulosResponse>>.Success(new List<ListarArticulosResponse>());
            }
            var response = articulos.Select(a => new ListarArticulosResponse(
                a.ArticuloId,
                a.Codigo,
                a.Nombre,
                a.Categoria!.Nombre,
                a.Stock,
                a.Estado.ToString(),
                a.Destacado,
                a.CantidadVentas
            )).ToList();
            return Result<List<ListarArticulosResponse>>.Success(response);
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
