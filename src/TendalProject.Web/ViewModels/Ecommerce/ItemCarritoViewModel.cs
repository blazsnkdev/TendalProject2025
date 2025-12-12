namespace TendalProject.Web.ViewModels.Ecommerce
{
    public class ItemCarritoViewModel
    {
        public Guid ItemId { get; set; }
        public Guid ArticuloId { get; set; }
        public string NombreArticulo { get; set; }
        public string Imagen { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public decimal SubTotal { get; set; }
    }
}
