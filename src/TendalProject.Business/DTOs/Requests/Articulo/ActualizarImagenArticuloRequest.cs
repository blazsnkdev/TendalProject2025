using Microsoft.AspNetCore.Http;

namespace TendalProject.Business.DTOs.Requests.Articulo
{
    public record ActualizarImagenArticuloRequest
    (
        Guid ArticuloId,
        IFormFile Imagen
    );
}
