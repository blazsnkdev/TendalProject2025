using TendalProject.Entities.Enum;

namespace TendalProject.Entities.Entidades
{
    public class Venta
    {
        public Guid VentaId { get; set; }
        public Guid PedidoId { get; set; }
        public Pedido Pedido { get; set; }
        public DateTime FechaVenta { get; set; }
        public TipoComprobante TipoComprobante { get; set; }
        public string NumeroComprobante { get; set; } = string.Empty;
        public MetodoPago MetodoPago { get; set; }
    }
}
