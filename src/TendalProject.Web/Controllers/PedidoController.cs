using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TendalProject.Business.DTOs.Requests.Pedido;
using TendalProject.Business.Interfaces;
using TendalProject.Common.Helpers;
using TendalProject.Common.Results;
using TendalProject.Entities.Enum;
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
            string? orden = "desc",
            EstadoPedido? estado = null
        )
        {
            var result = await _pedidoService.ObtenerPedidosAsync(
                fechaInicio,
                fechaFin,
                codigo,
                proximosAEntregar,
                ordenarPor,
                orden,
                estado
            );
            if (!result.IsSuccess || result.Value is null)
            {
                return HandleError(result.Error!);
            }
            ConfigurarFiltrosViewBag(fechaInicio,fechaFin,codigo,proximosAEntregar,ordenarPor,orden,estado);
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
        [Authorize(Roles = "Administrador, Cliente")]
        public async Task<IActionResult> Detalles(Guid pedidoId, int pagina = 1,int tamanioPagina = 10)
        {
            var result = await _pedidoService.ObtenerDetallesPedidoAsync(pedidoId);
            if(result.Value is null)
            {
                return HandleError(result.Error!);
            }
            var detalles = result.Value;
            var viewModel = detalles.Select(x => new DetallePedidoViewModel()
            {
                DetallePedidoId = x.DetallePedidoId,
                NombreArticulo = x.NombreArticulo,
                CodigoArticulo = x.CodigoArticulo,
                NombreCategoria = x.NombreCategoria,
                Cantidad = x.Cantidad,
                DescripcionArticulo = x.DescripcionArticulo,
                PrecioFinal = x.PrecioFinal,
                SubTotal = x.SubTotal
            }).ToList();
            var paginacion = PaginacionHelper.Paginacion(viewModel,pagina,tamanioPagina);
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
        private void ConfigurarFiltrosViewBag(
            DateTime? fechaInicio,
            DateTime? fechaFin,
            string? codigo,
            bool? proximosAEntregar,
            string? ordenarPor,
            string? orden,
            EstadoPedido? estado)
        {
            ViewBag.FechaInicio = fechaInicio?.ToString("yyyy-MM-dd");
            ViewBag.FechaFin = fechaFin?.ToString("yyyy-MM-dd");
            ViewBag.Codigo = codigo;
            ViewBag.ProximosAEntregar = proximosAEntregar;
            ViewBag.OrdenarPor = ordenarPor;
            ViewBag.Estado = estado;
            ViewBag.Orden = orden;

            ViewBag.Routes = new Dictionary<string, string?>
            {
                { "fechaInicio", fechaInicio?.ToString("yyyy-MM-dd") },
                { "fechaFin", fechaFin?.ToString("yyyy-MM-dd") },
                { "codigo", codigo },
                { "estado", estado?.ToString("D") },
                { "proximosAEntregar", proximosAEntregar?.ToString() },
                { "ordenarPor", ordenarPor },
                { "orden", orden }
            };
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstado(CambiarEstadoViewModel viewModel)
        {
            var result = await _pedidoService.ModificarEstadoAsync(
                new ModificarEstadoPedidoRequest(
                    viewModel.PedidoId, 
                    viewModel.EstadoSeleccionado));
            if (!result.IsSuccess)
            {
                return HandleError(result.Error!);
            }
            return RedirectToAction(nameof(Listar));
        }
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> Detalle(Guid pedidoId)
        {
            var result = await _pedidoService.ObtenerDetallePedidoPorClienteAsync(pedidoId);
            if (!result.IsSuccess)
            {
                return HandleError(result.Error!);
            }
            var value = result.Value!;
            var viewModel = new DetallePedidoClienteViewModel()
            {
                PedidoId = value.PedidoId,
                CodigoPedido = value.CodigoPedido,
                FechaRegistro = value.FechaRegistro,
                Estado =value.Estado,
                FechaEntrega = value.FechaEntrega ?? DateTime.MinValue,
                FechaEnvio = value.FechaEnvio,
                FechaPago = value.FechaPago ?? DateTime.MinValue,
                Igv = value.Igv,
                SubTotal = value.SubTotal,
                Total = value.Total,
                Items = value.CantidadItems,
                ItemsPedido = value.Items.Select(x=> new ItemsPedidoClienteViewModel()
                {
                    PedidoId = x.PedidoId,
                    ArticuloId =x.ArticuloId,
                    CodigoArticulo = x.CodigoArticulo,
                    Nombre = x.Nombre,
                    Descripcion = x.Descripcion,
                    Cantidad = x.Cantidad,
                    Precio = x.Precio,
                }).ToList()
            };
            return View(viewModel);
        }
        [Authorize(Roles = "Cliente")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Entregar(Guid pedidoId)
        {
            var result = await _pedidoService.MarcarEntregadoPedidoAsync(pedidoId);
            if (!result.IsSuccess)
            {
            }
            return RedirectToAction(nameof(Detalle), new { pedidoId = result.Value });
        }
        [HttpGet]//NOTE: endpoint para setear los estados en el selectList
        public IActionResult ObtenerEstadosDisponibles(string estadoActual)
        {
            var todosEstados = Enum.GetValues(typeof(EstadoPedido))
                .Cast<EstadoPedido>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = e.ToString()
                })
                .ToList();
            var estadosDisponibles = todosEstados
                .Where(e => e.Value != estadoActual)
                .ToList();

            return Json(new
            {
                estadosDisponibles,
                todosEstados
            });
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
