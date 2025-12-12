namespace TendalProject.Business.DTOs.Responses.Ecommerce
{
    public record CarritoResponse
    (
        Guid CarritoId,
        Guid ClienteId,
        List<ItemCarritoResponse> Items
    );
}
