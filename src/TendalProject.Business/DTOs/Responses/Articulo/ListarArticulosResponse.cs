namespace TendalProject.Business.DTOs.Responses.Articulo
{
    public record ListarArticulosResponse
    (
        Guid ArticuloId,
        string Codigo,
        string Nombre,
        string NombreCategoria,
        int Stock,
        string Estado,
        bool Destacado,
        int CantidadVentas
    );
}
