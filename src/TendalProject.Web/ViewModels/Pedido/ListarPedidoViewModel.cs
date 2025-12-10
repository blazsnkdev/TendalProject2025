namespace TendalProject.Web.ViewModels.Pedido
{
    public class ListarPedidoViewModel
    {
        public Guid PedidoId { get; set; }
        public string Codigo { get; set; } = "SIN CÓDIGO";
        public string NombreCliente { get; set; }= string.Empty;
        public DateTime FechaRegistro { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; } = null!;
        public int CantidadArticulos { get; set; }
    }
}
