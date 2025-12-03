namespace TendalProject.Business.DTOs.Responses.Categoria
{
    public record CategoriaResponse
    (
        Guid CategoriaId,
        string Nombre,
        string? Descripcion,
        string Estado,
        DateTime FechaRegistro
        );
}
