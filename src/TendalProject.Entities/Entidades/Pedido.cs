using TendalProject.Entities.Enum;

namespace TendalProject.Entities.Entidades
{
    public class Pedido
    {
        public Guid PedidoId { get; set; }
        public string Codigo { get; set; }= string.Empty;
        public Guid ClienteId { get; set; }
        public Cliente? Cliente { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime? FechaPago { get; set; }
        public DateTime? FechaEnvio { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Igv { get; set; }
        public decimal Total { get; set; }
        public EstadoPedido Estado { get; set; }
        public ICollection<DetallePedido> Detalles { get; set; } = new List<DetallePedido>();
    }
}
