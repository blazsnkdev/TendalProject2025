using System.ComponentModel.DataAnnotations;

namespace TendalProject.Web.ViewModels.Pedido
{
    public class DetallePedidoClienteViewModel
    {
        public Guid PedidoId { get; set; }
        public Guid ClienteId { get; set; }
        [Display(Name ="Código")]
        public string CodigoPedido { get; set; } = string.Empty;
        [Display(Name = "Registro")]
        public DateTime FechaRegistro { get; set; }
        [Display(Name = "Estado Actual")]
        public string Estado { get; set; } = string.Empty;
        [Display(Name = "Entrega")]
        public DateTime FechaEntrega { get; set; }
        [Display(Name = "Recepcionado")]
        public DateTime? FechaEnvio { get; set; }
        [Display(Name = "Pagado")]
        public DateTime FechaPago { get; set; }
        [Display(Name = "Igv %")]
        public decimal Igv { get; set; }
        [Display(Name = "Sub Total S/.")]
        public decimal SubTotal { get; set; }
        [Display(Name = "Total S/.")]
        public decimal Total { get; set; }
        [Display(Name = "Articulos Comprados")]
        public int Items { get; set; }
        public List<ItemsPedidoClienteViewModel> ItemsPedido { get; set; } = new List<ItemsPedidoClienteViewModel>();
    }
}
