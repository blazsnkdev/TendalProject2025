namespace TendalProject.Business.DTOs.Requests.Articulo
{
    public sealed record ReseñaArticuloRequest
    (
        Guid ArticuloId,
        Guid ClienteId,
        int Puntuacion,
        string Comentario
    );
}
