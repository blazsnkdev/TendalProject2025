using Microsoft.AspNetCore.Mvc.Rendering;
using TendalProject.Entities.Enum;

namespace TendalProject.Web.ViewModels.Pedido
{
    public class CambiarEstadoViewModel
    {
        public Guid PedidoId { get; set; }
        public EstadoPedido EstadoSeleccionado { get; set; }
        public IEnumerable<SelectListItem> Estados { get; set; } = [];
    }
}
