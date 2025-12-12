using TendalProject.Entities.Enum;

namespace TendalProject.Entities.Entidades
{
    public class Cliente
    {
        public Guid ClienteId { get; set; }
        public Guid UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        public string Nombre { get; set; } = null!;
        public string ApellidoPaterno { get; set; } = null!;
        public string ApellidoMaterno { get; set; } = null!;
        public string CorreoElectronico { get; set; } = null!;
        public string? NumeroCelular { get; set; }
        public DateOnly FechaNacimiento { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public EstadoCliente Estado { get; set; }
        public int CantidadPedidos { get; set; }
        public NivelCliente Nivel { get; set; }

        public Carrito? Carrito { get; set; }
        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
        public ICollection<Venta> Ventas { get; set; } = new List<Venta>();
        public ICollection<Reseña> Reseñas { get; set; } = new List<Reseña>();
    }
}
