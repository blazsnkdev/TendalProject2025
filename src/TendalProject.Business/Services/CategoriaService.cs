using Microsoft.EntityFrameworkCore;
using TendalProject.Business.DTOs.Requests.Categoria;
using TendalProject.Business.DTOs.Responses.Categoria;
using TendalProject.Business.Interfaces;
using TendalProject.Common.Results;
using TendalProject.Common.Time;
using TendalProject.Data.UnitOfWork;
using TendalProject.Entities.Entidades;
using TendalProject.Entities.Enum;

namespace TendalProject.Business.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IUnitOfWork _UoW;

        public CategoriaService(
            IDateTimeProvider dateTimeProvider,
            IUnitOfWork UoW
            )
        {
            _dateTimeProvider = dateTimeProvider;
            _UoW = UoW;
        }

        public async Task<Result<Guid>> ActualizarCategoriaAsync(ActualizarCategoriaRequest request)
        {
            try
            {
                if(request.CategoriaId == Guid.Empty)
                {
                    return Result<Guid>.Failure(Error.Validation("El Id de la categoría es inválido."));
                }
                if (string.IsNullOrWhiteSpace(request.Nombre) || string.IsNullOrWhiteSpace(request.Descripcion))
                {
                    return Result<Guid>.Failure(Error.Validation("Nombre y Descripción son nulos."));
                }
                var categoriaExistente = await _UoW.CategoriaRepository.GetByIdAsync(request.CategoriaId);
                if(categoriaExistente is null)
                {
                    return Result<Guid>.Failure(Error.NotFound("La categoría no existe."));
                }
                categoriaExistente.Nombre = request.Nombre.Trim();
                categoriaExistente.Descripcion = request.Descripcion?.Trim();
                await _UoW.SaveChangesAsync();
                return Result<Guid>.Success(categoriaExistente.CategoriaId);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Result<Guid>.Failure(Error.Conflict($"Conflicto al actualizar la categoría: {ex.Message}"));
            }
            catch(DbUpdateException ex)
            {
                return Result<Guid>.Failure(Error.Database($"Error al actualizar la categoría: {ex.Message}"));
            }
            catch (Exception ex)
            {
                return Result<Guid>.Failure(Error.Internal($"Error inesperado al actualizar la categoría: {ex.Message}"));
            }
        }

        public async Task<Result> ModificarEstadoAsync(Guid categoriaId)
        {
            try
            {
                var categoriaExistente = await _UoW.CategoriaRepository.GetByIdAsync(categoriaId);
                if (categoriaExistente is null)
                {
                    return Result.Failure(Error.NotFound("La categoría no existe."));
                }

                categoriaExistente.Estado =categoriaExistente.Estado == EstadoCategoria.Inactivo
                    ? EstadoCategoria.Activo
                    : EstadoCategoria.Inactivo;

                await _UoW.SaveChangesAsync();
                return Result.Success();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Result.Failure(Error.Conflict($"Conflicto al desactivar la categoría: {ex.Message}"));
            }
            catch (DbUpdateException ex)
            {
                return Result.Failure(Error.Database($"Error al desactivar la categoría: {ex.Message}"));
            }
            catch (Exception ex)
            {
                return Result.Failure(Error.Internal($"Error inesperado al desactivar la categoría: {ex.Message}"));
            }
        }

        public async Task<Result<List<CategoriaResponse>>> ObtenerCategoriasAsync()
        {
            var categorias = await _UoW.CategoriaRepository.GetAllAsync();
            var listaCategorias = categorias.Select(categoria => new CategoriaResponse
            (
                categoria.CategoriaId,
                categoria.Nombre,
                categoria.Descripcion,
                categoria.Estado.ToString(),
                categoria.FechaRegistro
            )).ToList();
            return Result<List<CategoriaResponse>>.Success(listaCategorias);
        }

        public async Task<Result<CategoriaResponse>> ObtenerDetalleCategoriaAsync(Guid categoriaId)
        {
            if(categoriaId == Guid.Empty)
            {
                return Result<CategoriaResponse>.Failure(Error.Validation("El Id de la categoría es inválido."));
            }
            var categoria = await _UoW.CategoriaRepository.GetByIdAsync(categoriaId);
            if (categoria is null)
            {
                return Result<CategoriaResponse>.Failure(Error.NotFound("La categoría no existe."));
            }

            var detalleCategoria = new CategoriaResponse
            (
                categoria.CategoriaId,
                categoria.Nombre,
                categoria.Descripcion,
                categoria.Estado.ToString(),
                categoria.FechaRegistro
            );
            return Result<CategoriaResponse>.Success(detalleCategoria);
        }

        public async Task<Result<Guid>> RegistrarCategoriaAsync(RegistrarCategoriaRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Nombre) || string.IsNullOrWhiteSpace(request.Descripcion))
                {
                    return Result<Guid>.Failure(Error.Validation("Nombre y Descripción son nulos."));
                }
                var nombre = request.Nombre.Trim().ToLower();
                if (await _UoW.CategoriaRepository.ExisteNombreAsync(nombre))
                {
                    return Result<Guid>.Failure(Error.Conflict("Ya existe una categoría con ese nombre."));
                }
                var categoria = new Categoria()
                {
                    CategoriaId = Guid.NewGuid(),
                    Nombre = nombre,
                    Descripcion = request.Descripcion,
                    Estado = EstadoCategoria.Activo,
                    FechaRegistro = _dateTimeProvider.GetDateTimeNow()
                };
                await _UoW.CategoriaRepository.AddAsync(categoria);
                await _UoW.SaveChangesAsync();
                return Result<Guid>.Success(categoria.CategoriaId);
            }
            catch (DbUpdateException ex)
            {
                return Result<Guid>.Failure(Error.Database($"Error al crear la categoría: {ex.Message}"));
            }
            catch (Exception ex)
            {
                return Result<Guid>.Failure(Error.Internal($"Error inesperado al crear la categoría: {ex.Message}"));
            }
        }
    }
}
