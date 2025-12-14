using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TendalProject.Business.Interfaces;
using TendalProject.Common.Helpers;
using TendalProject.Common.Results;
using TendalProject.Web.ViewModels.Pedido;

namespace TendalProject.Web.Controllers
{
    public class PedidoController : Controller
    {
        private readonly IPedidoService _pedidoService;

        public PedidoController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }
        [Authorize(Roles ="Administrador")]
        public async Task<IActionResult> Listar(
            int pagina = 1,
            int tamanioPagina = 10,
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null,
            string? codigo = null,
            bool? proximosAEntregar = null,
            string? ordenarPor = "fecha",
            string? orden = "desc"
        )
        {
            var result = await _pedidoService.ObtenerPedidosAsync(
                fechaInicio,
                fechaFin,
                codigo,
                proximosAEntregar,
                ordenarPor,
                orden
            );
            if (!result.IsSuccess || result.Value is null)
            {
                return HandleError(result.Error!);
            }
            ConfigurarFiltrosViewBag(fechaInicio,fechaFin,codigo,proximosAEntregar,ordenarPor,orden);
            var viewModel = result.Value.Select(p => new ListarPedidoViewModel
            {
                PedidoId = p.PedidoId,
                Codigo = p.Codigo,
                NombreCliente = p.NombreCliente,
                FechaRegistro = p.FechaRegistro,
                FechaEntrega = p.FechaEntrega,
                Total = p.Total,
                Estado = p.Estado,
                CantidadArticulos = p.CantidadArticulos
            }).ToList();
            var paginacion = PaginacionHelper.Paginacion(viewModel, pagina, tamanioPagina);
            return View(paginacion);
        }
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> Historial(int pagina = 1,int tamanioPagina = 10)
        {
            var clienteId = ObtenerClienteId();
            if(clienteId == Guid.Empty)
            {
                return HandleError(Error.Validation("No hay Cliente"));
            }
            var result = await _pedidoService.ObtenerPedidosPorClienteAsync(clienteId);
            var pedidos = result.Value;
            var viewModel = pedidos!.Select(p => new HistorialPedidosViewModel()
            {
                PedidoId = p.PedidoId,
                Codigo = p.Codigo,
                Total =p.MontoTotal,
                FechaRegistro = p.FechaRegistro,
                FechaEntrega = p.FechaEntrega,
                Estado = p.Estado,
                CantidadArticulos = p.CantidadArticulos
            }).ToList();
            var paginacion = PaginacionHelper.Paginacion(viewModel, pagina, tamanioPagina);
            return View(paginacion);
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
        private void ConfigurarFiltrosViewBag(DateTime? fechaInicio,DateTime? fechaFin,string? codigo,bool? proximosAEntregar,string? ordenarPor,string? orden)
        {
            ViewBag.FechaInicio = fechaInicio?.ToString("yyyy-MM-dd");
            ViewBag.FechaFin = fechaFin?.ToString("yyyy-MM-dd");
            ViewBag.Codigo = codigo;
            ViewBag.ProximosAEntregar = proximosAEntregar;
            ViewBag.OrdenarPor = ordenarPor;
            ViewBag.Orden = orden;

            ViewBag.Routes = new Dictionary<string, string?>
            {
                { "fechaInicio", fechaInicio?.ToString("yyyy-MM-dd") },
                { "fechaFin", fechaFin?.ToString("yyyy-MM-dd") },
                { "codigo", codigo },
                { "proximosAEntregar", proximosAEntregar?.ToString() },
                { "ordenarPor", ordenarPor },
                { "orden", orden }
            };
        }
        private IActionResult HandleError(Error error)
        {
            return error.Code switch
            {
                "ERROR_NOT_FOUND" => RedirectToAction("NotFoundPage", "Auth"),
                "ERROR_UNAUTHORIZED" => RedirectToAction("UnauthorizedPage", "Auth"),
                "ERROR_CONFLICT" => RedirectToAction("AccessDenied", "Auth"),
                "ERROR_VALIDATION" => RedirectToAction("AccessDenied", "Auth"),
                _ => RedirectToAction("AccessDenied", "Auth")
            };
        }
    }
}
