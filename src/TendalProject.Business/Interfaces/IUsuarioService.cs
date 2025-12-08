using TendalProject.Business.DTOs.Requests.Usuario;
using TendalProject.Business.DTOs.Responses.Usuario;
using TendalProject.Common.Results;

namespace TendalProject.Business.Interfaces
{
    public interface IUsuarioService
    {
        Task<Result<Guid>> RegistrarUsuarioAsync(RegistrarUsuarioRequest request);
        Task<Result<List<ListarUsuarioResponse>>> ObtenerUsuariosAsync();
        Task<Result<List<ListarRolesPorUsuarioResponse>>> ObtenerRolesPorUsuarioIdAsync(Guid usuarioId);
        Task<Result<DetalleUsuarioResponse>> ObtenerDetalleUsuarioAsync(Guid usuarioId);
    }
}
