namespace TendalProject.Web.ViewModels.Categoria
{
    public class ListarCategoriasViewModel
    {
        public Guid CategoriaId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }

    }
}
