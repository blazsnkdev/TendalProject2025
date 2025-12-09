namespace TendalProject.Entities.Entidades
{
    public class DetallePedido
    {
        public Guid DetallePedidoId { get; set; }
        public Guid PedidoId { get; set; }
        public Pedido Pedido { get; set; }
        public Guid ArticuloId { get; set; }
        public Articulo Articulo { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}
