namespace TendalProject.Web.ViewModels.Dashboard
{
    public class ArticuloMasVendidoViewModel
    {
        public Guid ArticuloId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int CantidadVentas { get; set; }
    }
}
