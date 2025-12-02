using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TendalProject.Business.DTOs.Requests.Auth;
using TendalProject.Business.DTOs.Responses.Auth;
using TendalProject.Business.Interfaeces;
using TendalProject.Common.Helpers;
using TendalProject.Common.Results;
using TendalProject.Common.Time;
using TendalProject.Data.UnitOfWork;
using TendalProject.Entities.Entidades;
using TendalProject.Entities.Enum;

namespace TendalProject.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _UoW;
        private readonly IDateTimeProvider _dateTimeProvider;
        public AuthService(
            IUnitOfWork uoW,
            IDateTimeProvider dateTimeProvider
            )
        {
            _UoW = uoW;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task SignInAsync(HttpContext httpContext, LoginValidoResponse response, bool recordar = true)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, response.usuarioId.ToString()),
                new Claim(ClaimTypes.Name, response.nombre),
                new Claim("SesionId", response.sesionId.ToString()),
                new Claim("UltimoLogin", response.ultimoLogin.ToString("O")),
                new Claim("InicioSesion", response.inicioSesion.ToString("O")),
            };

            foreach (var rol in response.roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var props = new AuthenticationProperties
            {
                IsPersistent = recordar,
                ExpiresUtc = recordar ? DateTimeOffset.UtcNow.AddDays(30) : DateTimeOffset.UtcNow.AddHours(12)
            };

            await httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                props);
        }

        public async Task<Result<LoginValidoResponse>> LoginAsync(CredencialesLoginRequest request)//TODO: por ahora devuelve un result pq no se que dto debe devolver XD
        {
            var usuario = await _UoW.UsuarioRepository.GetUsuarioConRolesPorEmailAsync(request.Email);
            if(usuario is null)
            {
                return Result<LoginValidoResponse>.Failure(Error.NotFound("Usuario no encontrado"));
            }
            
            if (!PasswordHelper.Verify(request.Password,usuario.PasswordHash))
            {
                return Result<LoginValidoResponse>.Failure(Error.Unauthorized());
            }
            var ultimaConexion = _dateTimeProvider.GetDateTimeNow();
            usuario.UltimaConexion = ultimaConexion;
            await _UoW.SaveChangesAsync();
            var loginValidoResponse = new LoginValidoResponse
            (
                sesionId: Guid.NewGuid(),
                usuarioId: usuario.UsuarioId,
                nombre: usuario.Email,
                ultimoLogin: usuario.UltimaConexion ?? ultimaConexion,
                inicioSesion: ultimaConexion,
                roles: usuario.UsuariosRoles.Select(ur => ur.Rol.Nombre).ToArray()
            );
            return Result<LoginValidoResponse>.Success(loginValidoResponse);
        }

        public async Task<Result> RegistroAsync(RegistroUsuarioRequest request)
        {
            var emailExiste = await _UoW.UsuarioRepository.ExisteEmailAsync(request.Email);
            if (emailExiste)
            {
                return Result.Failure(Error.Conflict("El email ya está registrado"));
            }
            if (!_dateTimeProvider.ValidarFecha(request.FechaNacimiento))
            {
                return Result.Failure(Error.Validation("La fecha de nacimiento no puede ser en el futuro"));
            }

            if (!_dateTimeProvider.ValidarMayoriaEdad(request.FechaNacimiento))
            {
                return Result.Failure(Error.Validation("El usuario debe ser mayor de edad"));
            }
            if(request.Password != request.ConfirmPassword)
            {
                return Result.Failure(Error.Validation("Las contraseñas no coinciden"));
            }
            Guid usuarioId = Guid.NewGuid();
            var cliente = new Cliente()
            {
                Nombre = request.Nombre,
                ApellidoPaterno = request.ApellidoPaterno,
                ApellidoMaterno = request.ApellidoMaterno,
                NumeroCelular = request.NumeroCelular,
                FechaNacimiento = request.FechaNacimiento,
                FechaCreacion = _dateTimeProvider.GetDateTimeNow(),
                CorreoElectronico = request.Email,
                Estado = EstadoCliente.Activo,
                UsuarioId = usuarioId
            };
            var usuario = new Usuario()
            {
                UsuarioId = usuarioId,
                Cliente = cliente,
                Email = request.Email,
                PasswordHash = PasswordHelper.Hash(request.Password),
                FechaCreacion = _dateTimeProvider.GetDateTimeNow(),
                Activo = true,
                UltimaConexion = null,
                IntentosFallidos = 0,
                UsuariosRoles = new List<UsuarioRol>()
                {
                    new UsuarioRol()
                    {
                        UsuarioId = usuarioId,
                        RolId = await _UoW.RolRepository.GetRolIdPorNombreAsync("Cliente")
                    }
                }
            };
            await _UoW.BeginTransactionAsync();
            try
            {
                await _UoW.ClienteRepository.AddAsync(cliente);
                await _UoW.UsuarioRepository.AddAsync(usuario);
                await _UoW.CommitTransactionAsync();
            }
            catch(Exception ex)
            {
                await _UoW.RollBackAsync();
                return Result.Failure(Error.Internal("Error al registrar el usuario: " + ex.Message));
            }
            return Result.Success();
        }

        public async Task LogoutAsync(HttpContext httpContext)
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
