namespace TendalProject.Web.ViewModels.Cliente
{
    public class ListarClienteViewModel
    {
        public Guid ClienteId { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string CorreoElectronico { get; set; }
        public string NumeroCelular { get; set; }
        public string Estado { get; set; }
    }
}
