using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TendalProject.Business.DTOs.Requests.Pedido;
using TendalProject.Business.Interfaces;
using TendalProject.Web.ViewModels.Ecommerce;

namespace TendalProject.Web.Controllers
{
    [Route("Pago")]
    public class PagoController : Controller
    {
        private readonly IPagoService _pagoService;
        private readonly IPedidoService _pedidoService;

        public PagoController(
            IPagoService pagoService,
            IPedidoService pedidoService)
        {
            _pagoService = pagoService;
            _pedidoService = pedidoService;
        }
        [Authorize(Roles = "Cliente")]
        [HttpPost]
        public async Task<IActionResult> Pagar(CarritoCheckoutViewModel viewModel)
        {
            var clienteId = ObtenerClienteId();
            if (viewModel.FechaEntrega.Date < DateTime.Today)
            {
                return BadRequest("La fecha de entrega no puede ser anterior a hoy");
            }
            var request = await _pedidoService.CrearPedidoPendienteAsync(new CrearPedidoPendienteRequest(clienteId, viewModel.FechaEntrega));
            if (request.IsSuccess && request.Value is not null)
            {
                var result = await _pagoService.CrearPrefernciaPagoAsync(request.Value.PedidoId);
                return Redirect(result.Value);
            }
            return View(viewModel);
        }
        [HttpGet("PagoExitoso", Name = "PagoExitoso")]
        public async Task<IActionResult> PagoExitoso(
            [FromQuery] string payment_id,
            [FromQuery] string collection_id,
            [FromQuery] string status)
        {
            var clienteId = ObtenerClienteId();
            var result = await _pagoService.ProcesarPagoExitosoAsync(clienteId, payment_id);
            if (!result.IsSuccess)
            {
                return RedirectToAction("Catalogo", "Ecommerce");
            }
            return View();
        }






        private Guid ObtenerClienteId()
        {
            var clienteIdClaim = User.Claims.FirstOrDefault(c => c.Type == "ClienteId")?.Value;
            if (string.IsNullOrEmpty(clienteIdClaim) || !Guid.TryParse(clienteIdClaim, out var clienteId))
            {
                return Guid.Empty;
            }
            return clienteId;
        }
    }
}
