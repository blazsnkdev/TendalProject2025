namespace TendalProject.Web.ViewModels.Pedido
{
    public class ItemsPedidoClienteViewModel
    {
        public Guid PedidoId { get; set; }
        public Guid ArticuloId { get; set; }
        public string CodigoArticulo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
    }
}
