using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TendalProject.Business.Interfaces;

namespace TendalProject.Web.Controllers
{
    public class PagoController : Controller
    {
        private readonly IEcommerceService _ecommerceService;
        private readonly IPagoService _pagoService;

        public PagoController(IEcommerceService ecommerceService, IPagoService pagoService)
        {
            _ecommerceService = ecommerceService;
            _pagoService = pagoService;
        }
        [Authorize(Roles = "Cliente")]
        [HttpPost]
        public async Task<IActionResult> Pagar()
        {
            var clienteId = ObtenerClienteId();
            var result = await _pagoService.CrearPrefernciaPagoAsync(clienteId);
            return Redirect(result.Value!);
        }
        [HttpGet("PagoExitoso")]
        public async Task<IActionResult> PagoExitoso([FromQuery] Dictionary<string, string> queryParams)
        {
            var payment_id = queryParams["payment_id"];
            var clienteId = ObtenerClienteId();
            await _pagoService.ProcesarPagoExitosoAsync(clienteId, payment_id);
            return RedirectToAction("Catalogo");
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
