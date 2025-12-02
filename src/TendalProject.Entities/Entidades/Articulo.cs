using System.ComponentModel.DataAnnotations.Schema;
using TendalProject.Entities.Enum;

namespace TendalProject.Entities.Entidades
{
    public class Articulo
    {
        public Guid ArticuloId { get; set; }
        public string Codigo { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioOferta { get; set; }
        public int Stock { get; set; }
        public Guid? CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }
        public Guid? ProveedorId { get; set; }
        public Proveedor? Proveedor { get; set; }
        public bool Destacado { get; set; }
        public EstadoArticulo Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string Imagen { get; set; } = string.Empty;
        public int CantidadVentas { get; set; }
    }
}
