using TendalProject.Entities.Enum;

namespace TendalProject.Entities.Entidades
{
    public class Proveedor
    {
        public Guid ProveedorId { get; set; }
        public string Nombre { get; set; } = null!;
        public string RazonSocial { get; set; } = string.Empty;
        public string Ruc { get; set; } = string.Empty;
        public string? Contacto { get; set; }
        public string Telefono { get; set; } = null!;
        public string? Email { get; set; }
        public string? Direccion { get; set; }
        public EstadoProveedor Estado { get; set; } 
        public DateTime FechaRegistro { get; set; }
        public ICollection<Articulo> Articulos { get; set; } = new List<Articulo>();
    }
}
