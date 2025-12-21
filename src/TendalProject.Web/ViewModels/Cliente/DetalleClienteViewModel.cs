namespace TendalProject.Web.ViewModels.Cliente
{
    public class DetalleClienteViewModel
    {
        public Guid ClienteId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string ApellidoPaterno { get; set; } = string.Empty;
        public string ApellidoMaterno { get; set; } = string.Empty;
        public string CorreoElectronico { get; set; } = string.Empty;
        public string NumeroCelular { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        public string Estado { get; set; } = string.Empty;
        public DateTime UltimaConexion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public decimal MontoTotalGastado { get; set; }
        public string Nivel { get; set; } = string.Empty;
        public int ComprasTotales { get; set; }
        public int PedidosPendientes { get; set; }
        public int PedidosProcesando { get; set; }
        public int PedidosEnviados { get; set; }
        public int PedidosEntregados { get; set; }
        public int PedidosCancelados { get; set; }
        public int PedidosPagados { get; set; }
    }
}
