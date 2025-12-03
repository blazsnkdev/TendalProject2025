using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TendalProject.Business.DTOs.Requests.Articulo;
using TendalProject.Business.Interfaces;
using TendalProject.Web.ViewModels.Articulo;

namespace TendalProject.Web.Controllers
{
    public class ArticuloController : Controller
    {
        private readonly IArticuloService _articuloService;
        private readonly ICategoriaService _categoriaService;
        private readonly IProveedorService _proveedorService;
        private readonly IWebHostEnvironment _env;
        public ArticuloController(
            IArticuloService articuloService,
            ICategoriaService categoriaService,
            IProveedorService proveedorService,
            IWebHostEnvironment env
            )
        {
            _articuloService = articuloService;
            _categoriaService = categoriaService;
            _proveedorService = proveedorService;
            _env = env;
        }
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Registrar()
        {
            await CargarSelectLists();
            return View(new RegistrarArticuloViewModel());
        }
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registrar(RegistrarArticuloViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                await CargarSelectLists();
                return View(viewModel);
            }
            var articuloId = Guid.NewGuid();
            string rutaImagen = string.Empty;
            if(viewModel.Imagen is not null)
            {
                var extension = Path.GetExtension(viewModel.Imagen.FileName);
                var nombreArchivo = $"{articuloId}{extension}";
                var folder = Path.Combine(_env.WebRootPath, "img", "Articulo");
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                var rutaArchivo = Path.Combine(folder, nombreArchivo);
                using var stream = new FileStream(rutaArchivo, FileMode.Create);
                await viewModel.Imagen.CopyToAsync(stream);
                rutaImagen = $"img/Articulo/{nombreArchivo}";
                var request = new RegistrarArticuloRequest(
                articuloId,
                viewModel.Nombre,
                viewModel.Descripcion,
                viewModel.Precio,
                viewModel.Stock,
                viewModel.CategoriaId,
                viewModel.ProveedorId,
                rutaImagen
                );
                var result = await _articuloService.RegistrarArticuloAsync(request);
                if (!result.IsSuccess)
                {
                    await CargarSelectLists();
                    return View(viewModel);
                }
            }
            return RedirectToAction("Index", "Home");
        }
        private async Task CargarSelectLists()
        {
            var categorias = await _categoriaService.ObtenerCategoriasActivasSelectListAsync();
            ViewBag.Categorias = categorias.IsSuccess
                ? new SelectList(categorias.Value, "CategoriaId", "Nombre")
                : new SelectList(Enumerable.Empty<SelectListItem>(), "CategoriaId", "Nombre");

            var proveedores = await _proveedorService.ObtenerProveedoresActivosSelectListAsync();
            ViewBag.Proveedores = proveedores.IsSuccess
                ? new SelectList(proveedores.Value, "ProveedorId", "Nombre")
                : new SelectList(Enumerable.Empty<SelectListItem>(), "ProveedorId", "Nombre");
        }
    }
}
