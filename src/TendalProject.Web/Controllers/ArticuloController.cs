using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TendalProject.Business.DTOs.Requests.Articulo;
using TendalProject.Business.Interfaces;
using TendalProject.Common.Helpers;
using TendalProject.Common.Results;
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
        public async Task<IActionResult> Listar(int pagina = 1, int tamanioPagina = 10)
        {
            var result = await _articuloService.ObtenerListaArticulosAsync();
            if (!result.IsSuccess || result.Value is null)
            {
                return View("Error");
            }
            var viewModel = result.Value.Select(a => new ListarArticulosViewModel()
            {
                ArticuloId = a.ArticuloId,
                Nombre = a.Nombre,
                NombreCategoria = a.NombreCategoria,
                Stock = a.Stock,
                Estado = a.Estado,
                Destacado = a.Destacado,
                Codigo = a.Codigo,
                CantidadVentas = a.CantidadVentas
            }).ToList();
            var paginacion = PaginacionHelper.Paginacion(viewModel, pagina, tamanioPagina);
            return View(paginacion);
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
                var allowed = new[] { "image/jpeg", "image/png", "image/webp" };
                if (!allowed.Contains(viewModel.Imagen.ContentType))
                {
                    ModelState.AddModelError("Imagen", "Formato no permitido.");
                    return View(viewModel);
                }
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
                return RedirectToAction(nameof(Detalle), new { articuloId = result.Value });
            }
            await CargarSelectLists();
            return View(viewModel);
        }
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Detalle(Guid articuloId)
        {
            var result = await _articuloService.DetalleArticuloAsync(articuloId);

            if (!result.IsSuccess || result.Value is null)
            {
                return RedirectToAction("Listar");
            }
            var rutaImagen = await _articuloService.ObtenerRutaImagenArticuloAsync(articuloId);
            if(!rutaImagen.IsSuccess || rutaImagen.Value is null)
            {
                return RedirectToAction("Listar");
            }
            var value = result.Value;
            var viewModel = new DetalleArticuloViewModel()
            {
                ArticuloId = articuloId,
                Codigo = value.Codigo,
                Nombre = value.Nombre,
                Descripcion = value.Descripcion,
                Precio = value.Precio,
                Stock = value.Stock,
                NombreCategoria = value.NombreCategoria,
                NombreProveedor = value.NombreProveedor,
                FechaRegistro = value.FechaRegistro,
                Imagen = rutaImagen.Value
            };

            return View(viewModel);
        }
        [Authorize(Roles = "Administrador")]
        public IActionResult ActualizarImagen(Guid articuloId)
        {
            return View(new ActualizarImagenArticuloViewModel { ArticuloId = articuloId });
        }
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActualizarImagen(ActualizarImagenArticuloViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            var request = new ActualizarImagenArticuloRequest(viewModel.ArticuloId, viewModel.Imagen!);
            var result = await _articuloService.ActualizarImagenArticuloAsync(request);
            if (!result.IsSuccess)
            {
                return View(viewModel);
            }
            return RedirectToAction(nameof(Detalle), new { articuloId = viewModel.ArticuloId });
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
