namespace TendalProject.Entities.Entidades
{
    public class Item
    {
        public Guid ItemId { get; set; }
        public Guid CarritoId { get; set; }
        public Carrito Carrito { get; set; }
        public Guid ArticuloId { get; set; }
        public Articulo Articulo { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}
