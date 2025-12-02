using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TendalProject.Business.DTOs.Requests.Categoria;
using TendalProject.Business.Interfaces;
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
        [Authorize(Roles ="Administrador")]
        public async Task<IActionResult> Detalle(Guid categoriaId)
        {
            var result = await _categoriaService.ObtenerDetalleCategoriaAsync(categoriaId);
            if (!result.IsSuccess)
            {
                return NotFound();//TODO: Manejar error adecuadamente
            }
            if(result.Value is null)
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
    }
}
