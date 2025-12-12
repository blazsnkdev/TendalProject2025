namespace TendalProject.Business.DTOs.Requests.Ecommerce
{
    public record EliminarItemCarritoRequest
    (
        Guid ClienteId,
        Guid ItemId
    );
}
