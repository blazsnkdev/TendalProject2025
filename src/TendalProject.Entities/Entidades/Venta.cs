using TendalProject.Entities.Enum;

namespace TendalProject.Entities.Entidades
{
    public class Venta
    {
        public Guid VentaId { get; set; }
        public Guid PedidoId { get; set; }
        public Pedido Pedido { get; set; }
        public Guid ClienteId { get; set; }
        public Cliente Cliente { get; set; }
        public DateTime FechaVenta { get; set; }
        public decimal Subtotal { get; set; }
        public decimal IGV { get; set; }
        public decimal ImporteTotal { get; set; }
        public TipoComprobante TipoComprobante { get; set; }
        public string NumeroComprobante { get; set; } = string.Empty;
        public MetodoPago MetodoPago { get; set; }
        public ICollection<DetalleVenta> Detalles { get; set; } = new List<DetalleVenta>();
    }
}
