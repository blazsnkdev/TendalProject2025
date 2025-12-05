using Microsoft.EntityFrameworkCore;
using TendalProject.Business.DTOs.Requests.Proveedor;
using TendalProject.Business.DTOs.Responses.Proveedor;
using TendalProject.Business.Interfaces;
using TendalProject.Common.Results;
using TendalProject.Common.Time;
using TendalProject.Common.Utils;
using TendalProject.Data.UnitOfWork;
using TendalProject.Entities.Entidades;
using TendalProject.Entities.Enum;

namespace TendalProject.Business.Services
{
    public class ProveedorService : IProveedorService
    {
        private readonly IUnitOfWork _UoW;
        private readonly IDateTimeProvider _dateTimeProvider;

        public ProveedorService(
            IUnitOfWork uoW,
            IDateTimeProvider dateTimeProvider
            )
        {
            _UoW = uoW;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Result> ActualizarProveedorAsync(ActualizarProveedorRequest request)
        {
            try
            {
                if(request.ProveedorId == Guid.Empty)
                {
                    return Result.Failure(Error.Validation("El ID del proveedor es obligatorio."));
                }
                if (!StringUtils.IsNullOrWhiteSpace(
                    request.Nombre,
                    request.Telefono,
                    request.Email,
                    request.Contacto,
                    request.Direccion))
                {
                    return Result<Guid>.Failure(Error.Validation("Todos los campos son obligatorios."));
                }
                if (!await _UoW.ProveedorRepository.ExisteRucAsync(request.Ruc))
                {
                    return Result<Guid>.Failure(Error.Validation("El RUC ya existe."));
                }
                if (!await _UoW.ProveedorRepository.ExisteEmailAsync(request.Email))
                {
                    return Result<Guid>.Failure(Error.Validation("El Email ya existe."));
                }
                if (!await _UoW.ProveedorRepository.ExisteTelefonoAsync(request.Telefono))
                {
                    return Result<Guid>.Failure(Error.Validation("El Teléfono ya existe."));
                }
                var proveedor = await _UoW.ProveedorRepository.GetByIdAsync(request.ProveedorId);
                if (proveedor == null)
                {
                    return Result<Guid>.Failure(Error.NotFound("Proveedor no encontrado."));
                }
                proveedor.Nombre = request.Nombre;
                proveedor.RazonSocial = request.RazonSocial;
                proveedor.Ruc = request.Ruc;
                proveedor.Contacto = request.Contacto;
                proveedor.Telefono = request.Telefono;
                proveedor.Email = request.Email;
                proveedor.Direccion = request.Direccion;

                await _UoW.SaveChangesAsync();
                return Result<Guid>.Success(proveedor.ProveedorId);
            }
            catch(DbUpdateConcurrencyException ex)
            {
                return Result<Guid>.Failure(Error.Conflict($"Conflicto al actualizar el proveedor: {ex.Message}"));
            }
            catch (DbUpdateException ex)
            {
                return Result<Guid>.Failure(Error.Database($"Error de base de datos al registrar el proveedor: {ex.Message}"));
            }
            catch (Exception ex)
            {
                return Result<Guid>.Failure(Error.Internal($"Ocurrió un error al registrar el proveedor: {ex.Message}"));
            }
            
        }

        public async Task<Result<ProveedorResponse>> DetalleProveedorAsync(Guid proveedorId)
        {
            if(proveedorId == Guid.Empty)
            {
                return Result<ProveedorResponse>.Failure(Error.Validation("El ID del proveedor es obligatorio."));
            }
            var proveedor = await _UoW.ProveedorRepository.GetByIdAsync(proveedorId);
            if (proveedor == null)
            {
                return Result<ProveedorResponse>.Failure(Error.NotFound("Proveedor no encontrado."));
            }
            var response = new ProveedorResponse(
                proveedorId,
                proveedor.Nombre,
                proveedor.RazonSocial,
                proveedor.Ruc,
                proveedor.Contacto,
                proveedor.Telefono,
                proveedor.Email,
                proveedor.Direccion,
                proveedor.Estado.ToString(),
                proveedor.FechaRegistro);
            return Result<ProveedorResponse>.Success(response);
        }

        public async Task<Result<List<ProveedorResponse>>> ObtenerProveedoresAsync()
        {
            var proveedores = await _UoW.ProveedorRepository.GetAllAsync();
            var response = new List<ProveedorResponse>();
            if (proveedores == null || !proveedores.Any())
            {
                return Result<List<ProveedorResponse>>.Success(response);
            }
            response = proveedores.Select(p => new ProveedorResponse(
                p.ProveedorId,
                p.Nombre,
                p.RazonSocial,
                p.Ruc,
                p.Contacto,
                p.Telefono,
                p.Email,
                p.Direccion,
                p.Estado.ToString(),
                p.FechaRegistro
            )).ToList();
            return Result<List<ProveedorResponse>>.Success(response);
        }

        public async Task<Result<Guid>> ModificarEstadoProveedorAsync(Guid proveedorId)
        {
            var proveedor = await _UoW.ProveedorRepository.GetByIdAsync(proveedorId);
            if(proveedor is null)
            {
                return Result<Guid>.Failure(Error.NotFound("Proveedor no encontrado."));
            }
            proveedor.Estado = proveedor.Estado == EstadoProveedor.Activo ? EstadoProveedor.Inactivo : EstadoProveedor.Activo;
            await _UoW.SaveChangesAsync();
            return Result<Guid>.Success(proveedor.ProveedorId);
        }

        public async Task<Result<List<ProveedorSelectListResponse>>> ObtenerProveedoresActivosSelectListAsync()
        {
            var proveedoresActivos = await _UoW.ProveedorRepository.GetAllProveedoresActivosAsync();
            var response = new List<ProveedorSelectListResponse>();
            response = proveedoresActivos.Select(p => new ProveedorSelectListResponse(
                p.ProveedorId,
                p.Nombre)).ToList();
            return Result<List<ProveedorSelectListResponse>>.Success(response);
        }

        public async Task<Result<Guid>> RegistrarProveedorAsync(RegistrarProveedorRequest request)
        {
            try
            {
                if(StringUtils.IsNullOrWhiteSpace(
                    request.Nombre,
                    request.Telefono,
                    request.Contacto,
                    request.Direccion,
                    request.Email))
                {
                    return Result<Guid>.Failure(Error.Validation("Todos los campos son obligatorios."));
                }
                if(await _UoW.ProveedorRepository.ExisteRucAsync(request.Ruc))
                {
                    return Result<Guid>.Failure(Error.Validation("El RUC ya existe."));
                }
                if(await _UoW.ProveedorRepository.ExisteEmailAsync(request.Email))
                {
                    return Result<Guid>.Failure(Error.Validation("El Email ya existe."));
                }
                if(await _UoW.ProveedorRepository.ExisteTelefonoAsync(request.Telefono))
                {
                    return Result<Guid>.Failure(Error.Validation("El Teléfono ya existe."));
                }
                var proveedorId = Guid.NewGuid();
                var proveedor = new Proveedor()
                {
                    ProveedorId = proveedorId,
                    Nombre = request.Nombre,
                    RazonSocial = request.RazonSocial,
                    Ruc = request.Ruc,
                    Contacto = request.Contacto,
                    Telefono = request.Telefono,
                    Email = request.Email,
                    Direccion = request.Direccion,
                    Estado = EstadoProveedor.Activo,
                    FechaRegistro = _dateTimeProvider.GetDateTimeNow()
                };
                await _UoW.ProveedorRepository.AddAsync(proveedor);
                await _UoW.SaveChangesAsync();
                return Result<Guid>.Success(proveedorId);
            }
            catch (DbUpdateException ex)
            {
                return Result<Guid>.Failure(Error.Database($"Error de base de datos al registrar el proveedor: {ex.Message}"));
            }
            catch (Exception ex)
            {
                return Result<Guid>.Failure(Error.Internal($"Ocurrió un error al registrar el proveedor: {ex.Message}"));
            }
        }
    }
}
