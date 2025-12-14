namespace TendalProject.Web.ViewModels.Pedido
{
    public class HistorialPedidosViewModel
    {
        public Guid PedidoId { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateOnly? FechaEntrega { get; set; }
        public string Estado { get; set; } = string.Empty;
        public int CantidadArticulos { get; set; }
    }
}
