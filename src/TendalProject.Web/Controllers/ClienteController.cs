using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TendalProject.Business.Interfaces;
using TendalProject.Common.Helpers;
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
    }
}
