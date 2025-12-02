using TendalProject.Entities.Enum;

namespace TendalProject.Entities.Entidades
{
    public class Categoria
    {
        public Guid CategoriaId { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public EstadoCategoria Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public ICollection<Articulo> Articulos { get; set; } = new List<Articulo>();
    }
}
