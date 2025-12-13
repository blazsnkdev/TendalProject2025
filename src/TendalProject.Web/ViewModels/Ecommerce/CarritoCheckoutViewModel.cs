namespace TendalProject.Web.ViewModels.Ecommerce
{
    public class CarritoCheckoutViewModel
    {
        public Guid CarritoId { get; set; }
        public Guid ClienteId { get; set; }

        public List<ItemCarritoViewModel> Items { get; set; } = new();

        public decimal Total { get; set; }
        public int CantidadTotal { get; set; }

        // Pago
        public string Moneda { get; set; } = "PEN";
        public string? TokenPago { get; set; }
    }
}
