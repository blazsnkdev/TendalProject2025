using Microsoft.AspNetCore.Http;
using TendalProject.Business.DTOs.Requests;
using TendalProject.Business.DTOs.Responses;
using TendalProject.Common.Results;

namespace TendalProject.Business.Interfaeces
{
    public interface IAuthService
    {
        Task<Result<LoginValidoResponse>> LoginAsync(CredencialesLoginRequest request);
        Task SignInAsync(HttpContext httpContext, LoginValidoResponse response, bool recordar = true);
        Task<Result> RegistroAsync(RegistroUsuarioRequest request);
    }
}
