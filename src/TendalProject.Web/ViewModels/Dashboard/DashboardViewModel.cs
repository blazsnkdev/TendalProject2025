namespace TendalProject.Web.ViewModels.Dashboard
{
    public class DashboardViewModel
    {
        public decimal VentasHoy { get; set; }
        public decimal VentasMes { get; set; }
        public int PedidosHoy { get; set; }
        public int PedidosPendientes { get; set; }
        public List<PedidosPorEstadoViewModel> PedidosPorEstado { get; set; } = new();
        public List<ArticuloMasVendidoViewModel> ArticulosMasVendidos { get; set; } = new();
        public List<VentasPorDiaViewModel> VentasPorDia { get; set; } = new();
        public List<string> Alertas { get; set; } = new();
    }
}
