using TendalProject.Business.DTOs.Requests.Usuario;
using TendalProject.Business.DTOs.Responses.Usuario;
using TendalProject.Business.Interfaces;
using TendalProject.Common.Helpers;
using TendalProject.Common.Results;
using TendalProject.Common.Time;
using TendalProject.Common.Utils;
using TendalProject.Data.UnitOfWork;
using TendalProject.Entities.Entidades;

namespace TendalProject.Business.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUnitOfWork _UoW;
        private readonly IDateTimeProvider  _dateTimeProvider;

        public UsuarioService(
            IUnitOfWork UoW,
            IDateTimeProvider dateTimeProvider)
        {
            _UoW = UoW;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Result<DetalleUsuarioResponse>> ObtenerDetalleUsuarioAsync(Guid usuarioId)
        {
            var usuario = await _UoW.UsuarioRepository.GetByIdAsync(usuarioId);
            if (usuario == null)
            {
                return Result<DetalleUsuarioResponse>.Failure(Error.NotFound("Usuario no encontrado."));
            }
            var roles = usuario.UsuariosRoles;
            var response = new DetalleUsuarioResponse(
                usuario.UsuarioId,
                usuario.Email,
                usuario.UltimaConexion,
                usuario.IntentosFallidos,
                usuario.Activo,
                usuario.FechaCreacion,
                roles.Select(r => r.Rol.Nombre).ToList()
            );
            return Result<DetalleUsuarioResponse>.Success(response);
        }

        public async Task<Result<List<ListarRolesPorUsuarioResponse>>> ObtenerRolesPorUsuarioIdAsync(Guid usuarioId)
        {
            var roles = await _UoW.UsuarioRepository.GetRolesPorUsuarioIdAsync(usuarioId);
            var response = roles.Select(r => new ListarRolesPorUsuarioResponse(
                r.Nombre,
                r.Descripcion
            )).ToList();
            return Result<List<ListarRolesPorUsuarioResponse>>.Success(response);
        }

        public async Task<Result<List<ListarUsuarioResponse>>> ObtenerUsuariosAsync()
        {
            var usuarios = await  _UoW.UsuarioRepository.GetAllAsync();
            var response = usuarios.Select(u => new ListarUsuarioResponse(
                u.UsuarioId,u.Email,u.PasswordHash,u.Activo, u.UltimaConexion
                )).ToList();
            return Result<List<ListarUsuarioResponse>>.Success(response);
        }

        public async Task<Result<Guid>> RegistrarUsuarioAsync(RegistrarUsuarioRequest request)
        {
            if (StringUtils.IsNullOrWhiteSpace(request.Email, request.Password, request.ConfirmarPassword))
            {
                return Result<Guid>.Failure(Error.Validation("Los campos no pueden estar vacíos."));
            }

            if (request.Password != request.ConfirmarPassword)
            {
                return Result<Guid>.Failure(Error.Validation("Las contraseñas no coinciden."));
            }

            var usuario = new Usuario
            {
                UsuarioId = Guid.NewGuid(),
                Email = request.Email,
                PasswordHash = PasswordHelper.Hash(request.Password),
                FechaCreacion = _dateTimeProvider.GetDateTimeNow()
            };

            await _UoW.UsuarioRepository.AddAsync(usuario);
            await _UoW.SaveChangesAsync();
            return Result<Guid>.Success(usuario.UsuarioId);
        }
    }
}
