using Microsoft.Build.Execution;

namespace TendalProject.Web.ViewModels.Categoria
{
    public class DetalleCategoriaViewModel
    {
        public Guid CategoriaId { get; set; }
        public string Nombre { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public string Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
