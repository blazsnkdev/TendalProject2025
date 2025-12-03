namespace TendalProject.Web.ViewModels.Articulo
{
    public class RegistrarArticuloViewModel
    {
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public Guid CategoriaId { get; set; }
        public Guid ProveedorId { get; set; }
        public IFormFile? Imagen { get; set; }
    }
}
