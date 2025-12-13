using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TendalProject.Business.Interfaces;

namespace TendalProject.Web.Controllers
{
    [Route("Pago")]
    public class PagoController : Controller
    {
        private readonly IPagoService _pagoService;

        public PagoController(IPagoService pagoService)
        {
            _pagoService = pagoService;
        }
        [Authorize(Roles = "Cliente")]
        [HttpPost]
        public async Task<IActionResult> Pagar()
        {
            var clienteId = ObtenerClienteId();
            var result = await _pagoService.CrearPrefernciaPagoAsync(clienteId);
            return Redirect(result.Value);
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
