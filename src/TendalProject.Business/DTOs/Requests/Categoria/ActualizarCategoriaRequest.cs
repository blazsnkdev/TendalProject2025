namespace TendalProject.Business.DTOs.Requests.Categoria
{
    public record ActualizarCategoriaRequest
    (
        Guid CategoriaId,
        string Nombre,
        string Descripcion
        );
}
