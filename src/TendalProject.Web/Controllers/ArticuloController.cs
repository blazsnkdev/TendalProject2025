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
        public async Task<IActionResult> Listar(bool mostrarInactivos = false, int pagina = 1, int tamanioPagina = 10)
        {
            var result = await _articuloService.ObtenerListaArticulosAsync();
            if (!result.IsSuccess || result.Value is null)
            {
                return HandleError(result.Error!);  
            }
            var articulos = result.Value
                .Where(a => mostrarInactivos ? a.Estado == "Inactivo" : a.Estado == "Activo")
                .ToList();

            var viewModel = articulos.Select(a => new ListarArticulosViewModel()
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
            ViewBag.MostrarInactivos = mostrarInactivos;
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
            if (viewModel.Imagen is not null)
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
                return HandleError(result.Error!);
            }
            var rutaImagen = await _articuloService.ObtenerRutaImagenArticuloAsync(articuloId);
            if (!rutaImagen.IsSuccess || rutaImagen.Value is null)
            {
                return HandleError(rutaImagen.Error!);
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
                Imagen = rutaImagen.Value,
                Estado = value.Estado
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
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Actualizar(Guid articuloId)
        {
            var result = await _articuloService.ObtenerArticuloActualizarAsync(articuloId);
            if (!result.IsSuccess || result.Value is null)
            {
                return HandleError(result.Error!);
            }
            var value = result.Value;
            var viewModel = new ActualizarArticuloViewModel()
            {
                ArticuloId = articuloId,
                Nombre = value.Nombre,
                Descripcion = value.Descripcion,
                Precio = value.Precio,
                PrecioOferta = value.PrecioOferta,
                Stock = value.Stock,
                Destacado = value.Destacado,
                CategoriaId = value.CategoriaId,
                ProveedorId = value.ProveedorId
            };
            await CargarSelectLists();
            return View(viewModel);
        }
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Actualizar(ActualizarArticuloViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                await CargarSelectLists();
                return View(viewModel);
            }
            var request = new ActualizarArticuloRequest(
                viewModel.ArticuloId,
                viewModel.Nombre,
                viewModel.Descripcion,
                viewModel.Precio,
                viewModel.PrecioOferta,
                viewModel.Stock,
                viewModel.Destacado,
                viewModel.CategoriaId,
                viewModel.ProveedorId
                );
            var result = await _articuloService.ActualizarArticuloAsync(request);
            if (!result.IsSuccess)
            {
                await CargarSelectLists();
                return View(viewModel);
            }
            return RedirectToAction(nameof(Detalle), new { articuloId = viewModel.ArticuloId });
        }
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ModificarEstado(Guid articuloId)
        {
            var result = await _articuloService.ModificarEstadoArticuloAsync(articuloId);
            if (!result.IsSuccess)
            {
                return HandleError(result.Error!);
            }
            return RedirectToAction(nameof(Detalle), new { articuloId = result.Value});
        }
        [Authorize(Roles = "Cliente")]
        public IActionResult Calificar(Guid articuloId)
        {
            var clienteId = ObtenerClienteId();
            var viewModel = new CalificarArticuloViewModel() { 
                ArticuloId = articuloId,
                ClienteId = clienteId
            };
            return View(viewModel);
        }
        [HttpPost]
        [Authorize(Roles = "Cliente")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Calificar(CalificarArticuloViewModel viewModel)
        {
            var request = new ReseñaArticuloRequest(
                viewModel.ArticuloId,
                viewModel.ClienteId,
                viewModel.Puntuacion,
                viewModel.Comentario);
            var result = await _articuloService.CalificarArticuloAsync(request);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Error!.Message);
                return View(viewModel);
            }
            return RedirectToAction("Catalogo", "Ecommerce");
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
