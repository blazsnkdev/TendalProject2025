using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TendalProject.Business.DTOs.Requests.Proveedor;
using TendalProject.Business.Interfaces;
using TendalProject.Common.Helpers;
using TendalProject.Web.ViewModels.Proveedor;

namespace TendalProject.Web.Controllers
{
    public class ProveedorController : Controller
    {
        private readonly IProveedorService _proveedorService;

        public ProveedorController(IProveedorService proveedorService)
        {
            _proveedorService = proveedorService;
        }
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Listar(int pagina = 1, int tamanioPagina = 10)
        {
            var result = await _proveedorService.ListarProveedoresAsync();
            if (!result.IsSuccess)
            {
                return View("Error", result.Error);
            }
            if (result.Value is null)
            {
                return View("Error", "No se encontraron proveedores.");
            }
            var viewModel = result.Value.Select(p => new ListarProveedoresViewModel
            {
                ProveedorId = p.ProveedorId,
                Nombre = p.Nombre,
                RazonSocial = p.RazonSocial,
                Ruc = p.Ruc,
                Contacto = p.Contacto!,
                Telefono = p.Telefono,
                Email = p.Email!,
                Direccion = p.Direccion!,
                Estado = p.Estado,
                FechaRegistro = p.FechaRegistro
            }).ToList();

            var paginacion = PaginacionHelper.Paginacion(viewModel, pagina, tamanioPagina);
            return View(paginacion);
        }
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Detalle(Guid proveedorId)
        {
            var result = await _proveedorService.DetalleProveedorAsync(proveedorId);
            if (!result.IsSuccess)
            {
                return View("Error", result.Error);//NOTE: che esto necesito cambiar real
            }
            if (result.Value is null)
            {
                return NotFound();
            }
            var viewModel = new DetalleProveedorViewModel
            {
                ProveedorId = result.Value.ProveedorId,
                Nombre = result.Value.Nombre,
                RazonSocial = result.Value.RazonSocial,
                Ruc = result.Value.Ruc,
                Contacto = result.Value.Contacto!,
                Telefono = result.Value.Telefono,
                Email = result.Value.Email!,
                Direccion = result.Value.Direccion!,
                Estado = result.Value.Estado,
                FechaRegistro = result.Value.FechaRegistro
            };
            return View(viewModel);
        }
        [Authorize(Roles = "Administrador")]
        public IActionResult Registrar() => View();
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registrar(RegistrarProveedorViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            var request = new RegistrarProveedorRequest(
                viewModel.Nombre,
                viewModel.RazonSocial,
                viewModel.Ruc,
                viewModel.Contacto,
                viewModel.Telefono,
                viewModel.Email,
                viewModel.Direccion
            );
            var result = await _proveedorService.RegistrarProveedorAsync(request);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, "Ocurrió un error al registrar el proveedor.");
                return View(viewModel);
            }
            return RedirectToAction(nameof(Detalle), new { proveedorId = result.Value });

        }
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Actualizar(Guid proveedorId)
        {
            var result = await _proveedorService.DetalleProveedorAsync(proveedorId);
            if (!result.IsSuccess)
            {
                return View("Error", result.Error);
            }
            if (result.Value is null)
            {
                return NotFound();
            }
            var viewModel = new ActualizarProveedorViewModel
            {
                ProveedorId = result.Value.ProveedorId,
                Nombre = result.Value.Nombre,
                RazonSocial = result.Value.RazonSocial,
                Ruc = result.Value.Ruc,
                Contacto = result.Value.Contacto!,
                Telefono = result.Value.Telefono,
                Email = result.Value.Email!,
                Direccion = result.Value.Direccion!
            };
            return View(viewModel);
        }
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Actualizar(ActualizarProveedorViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            var request = new ActualizarProveedorRequest(
                viewModel.ProveedorId,
                viewModel.Nombre,
                viewModel.RazonSocial,
                viewModel.Ruc,
                viewModel.Contacto,
                viewModel.Telefono,
                viewModel.Email,
                viewModel.Direccion
            );
            var result = await _proveedorService.ActualizarProveedorAsync(request);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, "Ocurrió un error al actualizar el proveedor.");
                return View(viewModel);
            }
            return RedirectToAction(nameof(Detalle), new { proveedorId = viewModel.ProveedorId });
        }
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(Guid proveedorId)
        {
            var result = await _proveedorService.ModificarEstadoProveedorAsync(proveedorId);
            if (!result.IsSuccess)
            {
                return View("Error", result.Error);
            }
            return RedirectToAction(nameof(Detalle), new { proveedorId = proveedorId });
        }
    }
}
