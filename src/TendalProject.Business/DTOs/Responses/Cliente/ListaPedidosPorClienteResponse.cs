namespace TendalProject.Business.DTOs.Responses.Cliente
{
    public record ListaPedidosPorClienteResponse
    (
        Guid PedidoId,
        string Codigo,
        DateTime FechaRegistro,
        decimal Total,
        string Estado
    );
}
