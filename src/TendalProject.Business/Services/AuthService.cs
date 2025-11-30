using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TendalProject.Business.DTOs.Requests;
using TendalProject.Business.DTOs.Responses;
using TendalProject.Business.Interfaeces;
using TendalProject.Common.Helpers;
using TendalProject.Common.Results;
using TendalProject.Common.Time;
using TendalProject.Data.UnitOfWork;

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
                claims.Add(new Claim(ClaimTypes.Role, rol));

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

        public async Task<Result<LoginValidoResponse>> LoginAsync(CredencialesLoginDto dto)//TODO: por ahora devuelve un result pq no se que dto debe devolver XD
        {
            var usuario = await _UoW.UsuarioRepository.GetUsuarioConRolesPorEmailAsync(dto.Email);
            if(usuario is null)
            {
                return Result<LoginValidoResponse>.Failure(Error.NotFound("Usuario no encontrado"));
            }
            
            if (!PasswordHelper.Verify(dto.Password,usuario.PasswordHash))
            {
                return Result<LoginValidoResponse>.Failure(Error.Unauthorized());
            }
            var ultimaConexion = _dateTimeProvider.GetDateTimeNow();
            usuario.UltimaConexion = ultimaConexion;
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
    }
}
