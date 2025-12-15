using NuGet.Protocol.Core.Types;

namespace TendalProject.Web.ViewModels.Cliente
{
    public class PerfilClienteViewModel
    {
        public Guid ClienteId { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string CorreoElectronico { get; set; }
        public string NumeroCelular { get; set; }
        public DateOnly FechaNacimiento { get; set; }
        public string Nivel { get; set; }
    }
}
