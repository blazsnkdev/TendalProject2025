namespace TendalProject.Web.ViewModels.Pedido
{
    public class DetallePedidoViewModel
    {
        public Guid DetallePedidoId { get; set; }
        public string NombreArticulo { get; set; } = string.Empty;
        public string CodigoArticulo { get; set; } = string.Empty;
        public string DescripcionArticulo { get; set; } = string.Empty;
        public string NombreCategoria { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PrecioFinal { get; set; }
        public decimal SubTotal { get; set; }
    }
}
