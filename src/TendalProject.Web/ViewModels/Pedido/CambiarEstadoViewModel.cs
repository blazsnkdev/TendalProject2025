using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using TendalProject.Entities.Enum;

namespace TendalProject.Web.ViewModels.Pedido
{
    public class CambiarEstadoViewModel
    {
        public Guid PedidoId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un nuevo estado")]
        [Display(Name = "Nuevo Estado")]
        public EstadoPedido EstadoSeleccionado { get; set; }

        [Display(Name = "Estado Actual")]
        public string EstadoActual { get; set; } = string.Empty;
        public IEnumerable<SelectListItem> EstadosDisponibles { get; set; } = new List<SelectListItem>();

        public List<SelectListItem> TodosEstados { get; set; } = new List<SelectListItem>();
    }
}

