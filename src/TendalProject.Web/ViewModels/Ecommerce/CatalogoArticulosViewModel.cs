namespace TendalProject.Web.ViewModels.Ecommerce
{
    public class CatalogoArticulosViewModel
    {
        public Guid ArticuloId { get; set; }
        public string NombreArticulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string NombreCategoria { get; set; } = string.Empty;
        public decimal PrecioUnitario { get; set; }
        public int Stock { get; set; }
        public decimal? PrecioOferta { get; set; }
        public string Imagen { get; set; } = string.Empty;
        public bool Disponible { get; set; }
        public double? Calificacion { get; set; }
        public bool EsNuevo { get; set; }
    }
}
