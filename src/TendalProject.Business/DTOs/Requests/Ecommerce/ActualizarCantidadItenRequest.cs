namespace TendalProject.Business.DTOs.Requests.Ecommerce
{
    public record ActualizarCantidadItenRequest
    (
        Guid ClienteId,
        Guid ItemId,
        int Cantidad
    );
}
