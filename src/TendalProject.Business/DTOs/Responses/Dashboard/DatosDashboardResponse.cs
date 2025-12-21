namespace TendalProject.Business.DTOs.Responses.Dashboard
{
    public record DatosDashboardResponse
    (
        decimal VentasHoy,
        decimal VentasMes,
        int PedidosHoy,
        int PedidosPendientes
    );
}
