using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TendalProject.Business.Interfaces;
using TendalProject.Common.Helpers;
using TendalProject.Common.Results;
using TendalProject.Web.ViewModels.Ecommerce;

namespace TendalProject.Web.Controllers
{
    public class EcommerceController : Controller
    {
        private readonly IEcommerceService _ecommerceService;
        private readonly ICategoriaService _categoriaService;

        public EcommerceController(
            IEcommerceService ecommerceService,
            ICategoriaService categoriaService)
        {
            _ecommerceService = ecommerceService;
            _categoriaService = categoriaService;
        }
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> Catalogo(
            Guid? categoriaId,
            string? q,
            decimal? minPrecio,
            decimal? maxPrecio,
            bool? oferta,
            bool? disponible,
            double? minRating,
            bool? nuevo,
            string? orden,
            int pagina = 1,
            int tamanioPagina = 10
        )
        {
            var result = await _ecommerceService.ObtenerCatalogoFiltradoAsync(
                categoriaId, q, minPrecio, maxPrecio, oferta,
                disponible, minRating, nuevo, orden
            );
            if (result.Value is null)
            {
                return HandleError(result.Error!);
            }
            var viewModel = result.Value.Select(c => new CatalogoArticulosViewModel()
            {
                ArticuloId = c.ArticuloId,
                NombreArticulo = c.NombreArticulo,
                Descripcion= c.Descripcion,
                NombreCategoria = c.NombreCategoria,
                PrecioUnitario = c.PrecioUnitario,
                Stock = c.Stock,
                PrecioOferta = c.PrecioOferta,
                Imagen = c.Imagen,
                Disponible = c.Disponible,
                Calificacion = c.Calificacion,
                EsNuevo = c.EsNuevo
            }).ToList();
            var paginacion = PaginacionHelper.Paginacion(viewModel,pagina,tamanioPagina);
            await CargarCategoriaSelectList();
            return View(paginacion);
        }

        [Authorize(Roles = "Administrador, Cliente")]
        public async Task<IActionResult> Detalle(Guid articuloId)
        {
            var result = await _ecommerceService.ObtenerArticuloSelccionadoAsync(articuloId);
            if (!result.IsSuccess || result.Value is null)
            {
                return HandleError(result.Error!);
            }
            var value = result.Value;
            var viewModel = new DetalleArticuloSeleccionadoViewModel()
            {
                ArticuloId = value.ArticuloId,
                NombreArticulo = value.NombreArticulo,
                NombreCategoria = value.NombreCategoria,
                Descripcion = value.Descripcion,
                Stock = value.Stock,
                Precio = value.PrecioFinal,
                Imagen = value.Imagen,
                Calificacion = value.Calificacion,
            };
            return View(viewModel);
        }
        [Authorize(Roles ="Administrador, Cliente")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Agregar(Guid articuloId, int cantidad)
        {
            return RedirectToAction(nameof(Carrito));
        }
        public async Task<IActionResult> Carrito()
        {
            return View();
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
        private async Task CargarCategoriaSelectList()
        {
            var categorias = await _categoriaService.ObtenerCategoriasActivasSelectListAsync();
            ViewBag.Categorias = categorias.IsSuccess
                ? new SelectList(categorias.Value, "CategoriaId", "Nombre")
                : new SelectList(Enumerable.Empty<SelectListItem>(), "CategoriaId", "Nombre");

        }
    }
}
