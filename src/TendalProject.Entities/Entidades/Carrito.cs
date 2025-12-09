namespace TendalProject.Entities.Entidades
{
    public class Carrito
    {
        public Guid CarritoId { get; set; }
        public Guid ClienteId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public Cliente? Cliente { get; set; }
        public List<Item> Items { get; set; } = new List<Item>();
    }
}
