using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TendalProject.Business.DTOs.Requests.Categoria;
using TendalProject.Business.Interfaces;
using TendalProject.Business.Services;
using TendalProject.Common.Helpers;
using TendalProject.Web.ViewModels.Categoria;

namespace TendalProject.Web.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriaController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Detalle(Guid categoriaId)
        {
            var result = await _categoriaService.ObtenerDetalleCategoriaAsync(categoriaId);
            if (!result.IsSuccess)
            {
                return NotFound();//TODO: Manejar error adecuadamente
            }
            if (result.Value is null)
            {
                return NotFound();
            }
            var viewModel = new DetalleCategoriaViewModel
            {
                CategoriaId = result.Value.CategoriaId,
                Nombre = result.Value.Nombre,
                Descripcion = result.Value.Descripcion!,
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
        public async Task<IActionResult> Registrar(RegistrarCategoriaViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            var request = new RegistrarCategoriaRequest(viewModel.Nombre, viewModel.Descripcion);
            var result = await _categoriaService.RegistrarCategoriaAsync(request);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, "Ocurrió un error al registrar la categoría.");
                return View(viewModel);
            }
            return RedirectToAction(nameof(Detalle), new { categoriaId = result.Value });
        }
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Listar(bool mostrarInactivos = false,int pagina = 1, int tamanioPagina = 10)
        {
            var result = await _categoriaService.ObtenerCategoriasAsync();
            if (!result.IsSuccess || result.Value is null)
            {
                return View("Error");
            }
            var categorias = result.Value
                .Where(c => mostrarInactivos ? c.Estado == "Inactivo" : c.Estado == "Activo")
                .ToList();

            var viewModel = categorias.Select(c => new ListarCategoriasViewModel()
            {
                CategoriaId = c.CategoriaId,
                Nombre = c.Nombre,
                Descripcion = c.Descripcion!,
                Estado = c.Estado,
                FechaRegistro = c.FechaRegistro
            }).ToList();
            var paginacion = PaginacionHelper.Paginacion(viewModel, pagina, tamanioPagina);
            ViewBag.MostrarInactivos = mostrarInactivos;
            return View(paginacion);
        }
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Actualizar(Guid categoriaId)
        {
            var result = await _categoriaService.ObtenerDetalleCategoriaAsync(categoriaId);
            if (!result.IsSuccess)
            {
                return NotFound();//TODO: Manejar error adecuadamente
            }
            if (result.Value is null)
            {
                return NotFound();
            }
            var viewModel = new ActualizarCategoriaViewModel
            {
                CategoriaId = result.Value.CategoriaId,
                Nombre = result.Value.Nombre,
                Descripcion = result.Value.Descripcion!
            };
            return View(viewModel);
        }
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Actualizar(ActualizarCategoriaViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            var request = new ActualizarCategoriaRequest(viewModel.CategoriaId, viewModel.Nombre, viewModel.Descripcion);
            var result = await _categoriaService.ActualizarCategoriaAsync(request);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, "Ocurrió un error al actualizar la categoría.");
                return View(viewModel);
            }
            return RedirectToAction(nameof(Detalle), new { categoriaId = viewModel.CategoriaId });
        }
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ModificarEstado(Guid categoriaId)
        {
            var result = await _categoriaService.ModificarEstadoAsync(categoriaId);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, "Ocurrió un error al eliminar la categoría.");
                return RedirectToAction(nameof(Listar));
            }
            return RedirectToAction(nameof(Listar));
        }
    }
}
