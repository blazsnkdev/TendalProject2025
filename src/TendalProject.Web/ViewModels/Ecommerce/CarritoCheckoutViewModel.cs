namespace TendalProject.Web.ViewModels.Ecommerce
{
    public class CarritoCheckoutViewModel
    {
        public Guid CarritoId { get; set; }
        public Guid ClienteId { get; set; }

        public List<ItemCarritoViewModel> Items { get; set; } = new();

        public decimal Total { get; set; }
        public int CantidadTotal { get; set; }
        public DateTime FechaEntrega { get; set; }//

    }
}
