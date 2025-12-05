using TendalProject.Business.DTOs.Requests.Usuario;
using TendalProject.Common.Results;

namespace TendalProject.Business.Interfaces
{
    public interface IUsuarioService
    {
        Task<Result<Guid>> RegistrarUsuarioAsync(RegistrarUsuarioRequest request);
    }
}
