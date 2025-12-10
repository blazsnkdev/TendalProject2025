namespace TendalProject.Web.ViewModels.Cliente
{
    public class ListarPedidosClienteViewModel
    {
        public Guid PedidoId { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string Estado { get; set; } = string.Empty;
    }
}
