namespace TendalProject.Entities.Entidades
{
    public class Reseña
    {
        public Guid ReseñaId { get; set; }
        public Guid ArticuloId { get; set; }
        public Articulo Articulo { get; set; }
        public Guid ClienteId { get; set; }
        public Cliente Cliente { get; set; }
        public int Puntuacion { get; set; }
        public string Comentario { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
    }
}
