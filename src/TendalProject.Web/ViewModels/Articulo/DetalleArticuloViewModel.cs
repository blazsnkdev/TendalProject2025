using Microsoft.Build.Execution;

namespace TendalProject.Web.ViewModels.Articulo
{
    public class DetalleArticuloViewModel
    {
        public Guid ArticuloId { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string NombreCategoria { get; set; }
        public string NombreProveedor { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string Imagen { get; set; }
    }
}
