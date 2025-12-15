using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TendalProject.Business.DTOs.Requests.Cliente;
using TendalProject.Business.Interfaces;
using TendalProject.Common.Helpers;
using TendalProject.Common.Results;
using TendalProject.Web.ViewModels.Cliente;

namespace TendalProject.Web.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IClienteService _clienteService;
        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Listar(string? nombre, bool mostrarInactivos = false, int pagina = 1, int tamanioPagina = 10)
        {
            var result = await _clienteService.ListarClientesAsync(nombre);
            var clientes = result.Value!
                .Where(c => mostrarInactivos ? c.Estado == "Inactivo" : c.Estado == "Activo").ToList();
            var viewModel = clientes.Select(c => new ListarClienteViewModel()
            {
                ClienteId = c.ClienteId,
                Nombre = c.Nombre,
                Apellidos = c.Apellidos,
                CorreoElectronico = c.CorreoElectronico,
                NumeroCelular = c.NumeroCelular,
                Estado = c.Estado
            });
            var paginacion = PaginacionHelper.Paginacion(viewModel, pagina, tamanioPagina);
            ViewBag.MostrarInactivos = mostrarInactivos;
            ViewBag.Nombre = nombre;
            return View(paginacion);
        }
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Detalle(Guid clienteId)
        {
            var result = await _clienteService.ObtenerDetalleClienteAsync(clienteId);
            if (!result.IsSuccess)
            {
                return HandleError(result.Error!);
            }
            var cliente = result.Value!;
            var viewModel = new DetalleClienteViewModel()
            {
                ClienteId = cliente.ClienteId,
                Nombre = cliente.Nombre,
                ApellidoPaterno = cliente.ApellidoPaterno,
                ApellidoMaterno = cliente.ApellidoMaterno,
                CorreoElectronico = cliente.CorreoElectronico,
                NumeroCelular = cliente.NumeroCelular,
                FechaNacimiento = cliente.FechaNacimiento.ToDateTime(new TimeOnly(0, 0)),
                Estado = cliente.Estado,
                UltimaConexion = cliente.UltimaConexion ?? DateTime.MinValue,
                FechaCreacion = cliente.FechaCreacion,
                FechaModificacion = cliente.FechaModificacion ?? DateTime.MinValue,
                MontoTotalGastado = cliente.MontoTotalGastado,
                Nivel = cliente.Nivel,
                ComprasTotales = cliente.CantidadCompras,
                PedidosPendientes = cliente.TotalPedidosPendientes,
                PedidosProcesando = cliente.TotalPedidosProcesando,
                PedidosEnviados = cliente.TotalPedidosEnviados,
                PedidosEntregados = cliente.TotalPedidosEntregados,
                PedidosCancelados = cliente.TotalPedidosCancelados
            };
            return View(viewModel);
        }
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Pedidos(Guid clienteId,string? estado, int pagina = 1, int tamanioPagina = 10)
        {
            var result = await _clienteService.ObtenerPedidosPorClienteAsync(clienteId);
            if (!result.IsSuccess)
            {
                return HandleError(result.Error!);
            }
            var pedidos = result.Value!
                .Where(p => p.Estado == estado);
            var viewModel = pedidos.Select(p => new ListarPedidosClienteViewModel()
            {
                PedidoId = p.PedidoId,
                Codigo = p.Codigo,
                Total = p.Total,
                FechaRegistro = p.FechaRegistro,
                Estado = p.Estado
            });
            var paginacion = PaginacionHelper.Paginacion(viewModel, pagina, tamanioPagina);
            ViewBag.ClienteId = clienteId;
            return View(paginacion);
        }
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ModificarEstado(Guid clienteId)
        {
            var result = await _clienteService.ModificarEstadoClienteAsync(clienteId);
            if (!result.IsSuccess)
            {
                return HandleError(result.Error!);
            }
            return RedirectToAction(nameof(Detalle), new { clienteId = result.Value });
        }
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> Perfil()
        {
            var clienteId = ObtenerClienteId();
            if(clienteId == Guid.Empty)
            {
                return RedirectToAction("Login", "Auth");
            }
            var result = await _clienteService.ObtenerPerfilClienteAsync(clienteId);
            if (!result.IsSuccess)
            {
                return HandleError(result.Error!);
            }
            var value = result.Value!;
            var viewModel = new PerfilClienteViewModel()
            {
                ClienteId = value.ClienteId,
                Nombre = value.Nombre,
                ApellidoPaterno = value.ApellidoPaterno,
                ApellidoMaterno = value.ApellidoMaterno,
                CorreoElectronico = value.CorreoElectronico,
                NumeroCelular = value.NumeroCelular,
                FechaNacimiento = value.FechaNacimiento,
                Nivel = value.Nivel

            };
            return View(viewModel);
        }
        [HttpPost]
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> ActualizarPassword(CambiarPasswordViewModel viewModel)
        {
            var clienteId = ObtenerClienteId();
            if(clienteId == Guid.Empty)
            {
                return RedirectToAction("Login", "Auh");
            }
            var request = new ActualizarPasswordClienteRequest(
                clienteId,
                viewModel.PasswordActual,
                viewModel.NuevoPassword,
                viewModel.ConfirmarPassword);
            var result = await  _clienteService.ActualizarPasswordAsync(request);
            if (!result.IsSuccess)
            {
                TempData["ErrorPasswordMensaje"] = result.Error!.Message;
                TempData["AbrirModal"] = true;
                return RedirectToAction(nameof(Perfil));
            }
            return RedirectToAction(nameof(Perfil));
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
