namespace TendalProject.Web.ViewModels.Articulo
{
    public class ListarArticulosViewModel
    {
        public Guid ArticuloId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public string NombreCategoria { get; set; } = string.Empty;
        public int Stock { get; set; }
        public string Estado { get; set; } = string.Empty;
        public bool Destacado { get; set; }
        public int CantidadVentas { get; set; }
    }
}
