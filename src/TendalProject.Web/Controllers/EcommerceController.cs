using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TendalProject.Business.DTOs.Requests.Ecommerce;
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
        public async Task<IActionResult> Agregar(Guid articuloId, int cantidad, decimal precioFinal)
        {
            var clienteId = User.FindFirst("ClienteId")?.Value;
            if(clienteId is null || string.IsNullOrWhiteSpace(clienteId))
            {
                return RedirectToAction("Login", "Auth");
            }
            var request = new SeleccionarArticuloRequest(articuloId,cantidad,clienteId,precioFinal);
            var result = await _ecommerceService.AgregarItemAlCarritoAsync(request);
            if (!result.IsSuccess)
            {
                return HandleError(result.Error!);
            }
            //NOTE: el carrito necesito el id
            return RedirectToAction(nameof(Carrito), new {clienteId = result.Value });
        }
        public async Task<IActionResult> Carrito(Guid clienteId)
        {
            var result = await _ecommerceService.ObtenerCarritoAsync(clienteId);
            var value = result.Value!;
            var itemsViewModel = value.Items.Select(x => new ItemCarritoViewModel
            {
                ItemId = x.ItemId,
                ArticuloId = x.ArticuloId,
                NombreArticulo = x.NombreArticulo,
                Cantidad = x.Cantidad,
                Precio = x.Precio,
                Imagen = x.Imagen,
                SubTotal = x.SubTotal
            }).ToList();
            var viewModel = new CarritoViewModel()
            {
                CarritoId = value.CarritoId,
                ClienteId = value.ClienteId,
                Items = itemsViewModel,
            };
            return View(viewModel);
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
