namespace TendalProject.Business.DTOs.Responses.Categoria
{
    public record DetalleCategoriaResponse
    (
        Guid CategoriaId,
        string Nombre,
        string? Descripcion,
        string Estado,
        DateTime FechaRegistro
        );
}
