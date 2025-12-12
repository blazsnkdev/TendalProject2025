namespace TendalProject.Business.DTOs.Requests.Ecommerce
{
    public record SeleccionarArticuloRequest
    (
        Guid ArticuloId,
        int Cantidad
    );
}
