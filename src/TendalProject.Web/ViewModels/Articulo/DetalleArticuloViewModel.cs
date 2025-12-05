using Microsoft.Build.Execution;

namespace TendalProject.Web.ViewModels.Articulo
{
    public class DetalleArticuloViewModel
    {
        public Guid ArticuloId { get; set; }
        public string Codigo { get; set; }=string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string NombreCategoria { get; set; } = string.Empty;
        public string NombreProveedor { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
        public string Imagen { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
    }
}
