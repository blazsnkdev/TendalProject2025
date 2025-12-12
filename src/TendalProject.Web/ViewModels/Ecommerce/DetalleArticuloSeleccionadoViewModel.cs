namespace TendalProject.Web.ViewModels.Ecommerce
{
    public class DetalleArticuloSeleccionadoViewModel
    {
        public Guid ArticuloId { get; set; }
        public string NombreArticulo { get; set; } = string.Empty;
        public string NombreCategoria { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal? Precio { get; set; }//NOTE: esto va ser el precio elegido
        public int Stock { get; set; }
        public string Imagen { get; set; } = string.Empty;
        public int Calificacion { get; set; }
    }
}
