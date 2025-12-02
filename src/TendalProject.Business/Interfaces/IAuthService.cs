using Microsoft.AspNetCore.Http;
using TendalProject.Business.DTOs.Requests.Auth;
using TendalProject.Business.DTOs.Responses.Auth;
using TendalProject.Common.Results;

namespace TendalProject.Business.Interfaeces
{
    public interface IAuthService
    {
        Task<Result<LoginValidoResponse>> LoginAsync(CredencialesLoginRequest request);
        Task SignInAsync(HttpContext httpContext, LoginValidoResponse response, bool recordar = true);
        Task<Result> RegistroAsync(RegistroUsuarioRequest request);
        Task LogoutAsync(HttpContext httpContext);
    }
}
