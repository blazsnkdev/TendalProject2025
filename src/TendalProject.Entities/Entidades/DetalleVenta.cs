namespace TendalProject.Entities.Entidades
{
    public class DetalleVenta
    {
        public Guid DetalleVentaId { get; set; }
        public Guid VentaId { get; set; }
        public Venta Venta { get; set; }
        public Guid ArticuloId { get; set; }
        public Articulo Articulo { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal => Cantidad * PrecioUnitario;
    }
}

