namespace TendalProject.Web.ViewModels.Ecommerce
{
    public class CarritoViewModel
    {
        public Guid CarritoId { get; set; }
        public Guid ClienteId { get; set; }
        public List<ItemCarritoViewModel> Items { get; set; } = new();
        public decimal PrecioTotal => Items.Sum(i => i.SubTotal);
        public int CantidadTotal => Items.Sum(i => i.Cantidad);
    }
}
