using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TendalProject.Business.DTOs.Requests.Usuario;
using TendalProject.Business.Interfaces;
using TendalProject.Common.Helpers;
using TendalProject.Common.Results;
using TendalProject.Web.ViewModels.Usuario;


namespace TendalProject.Web.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }
        [Authorize(Roles = "Administrador")]
        public IActionResult Registrar()
        {
            return View(new RegistrarUsuarioViewModel());
        }
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registrar(RegistrarUsuarioViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await _usuarioService.RegistrarUsuarioAsync(new RegistrarUsuarioRequest(
                model.Email,
                model.Password,
                model.ConfirmarPassword
            ));
            if (!result.IsSuccess)
            {
                return View(model);
            }
            return RedirectToAction("Index", "Home");
        }
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Listar(bool mostrarInactivos = false, int pagina = 1, int tamanioPagina = 10)
        {
            var result = await _usuarioService.ObtenerUsuariosAsync();
            if (!result.IsSuccess || result.Value is null)
            {
                return HandleError(result.Error!);
            }
            var usuarios = result.Value
                .Where(u => mostrarInactivos ? u.Activo == false : true).ToList();
            var viewModel = usuarios.Select(u => new ListarUsuarioViewModel
            {
                UsuarioId = u.UsuarioId,
                Email = u.Email,
                Activo = u.Activo,
                UltimaConexion = u.UltimaConexion
            }).ToList();
            var paginacion = PaginacionHelper.Paginacion(viewModel, pagina, tamanioPagina);
            ViewBag.MostrarInactivos = mostrarInactivos;
            return View(paginacion);
        }
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Roles(Guid usuarioId, int pagina = 1, int tamanioPagina = 10)
        {
            var result = await _usuarioService.ObtenerRolesPorUsuarioIdAsync(usuarioId);
            if (!result.IsSuccess || result.Value is null)
            {
                return HandleError(result.Error!);
            }
            var viewModel = result.Value.Select(r => new ListarRolPorUsuarioViewModel
            {
                Nombre = r.Nombre,
                Descripcion = r.Descripcion
            }).ToList();
            ViewBag.UsuarioId = usuarioId;
            var paginacion = PaginacionHelper.Paginacion(viewModel, pagina, tamanioPagina);
            return View(paginacion);
        }
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Detalle(Guid usuarioId)
        {
            var result = await _usuarioService.ObtenerDetalleUsuarioAsync(usuarioId);
            if (!result.IsSuccess || result.Value is null)
            {
                return HandleError(result.Error!);
            }
            var usuario = result.Value;
            var viewModel = new DetalleUsuarioViewModel
            {
                UsuarioId = usuario.UsuarioId,
                Email = usuario.Email,
                UltimaConexion = usuario.UltimaConexion,
                IntentosFallidos = usuario.IntentosFallidos,
                Activo = usuario.Activo,
                FechaCreacion = usuario.FechaCreacion,
                Roles = usuario.Roles
            };
            return View(viewModel);
        }
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ModificarEstado(Guid usuarioId)
        {
            var result = await _usuarioService.ActualizarEstadoUsuarioAsync(usuarioId);
            if (!result.IsSuccess)
            {
                return HandleError(result.Error!);
            }
            return RedirectToAction(nameof(Detalle), new {usuarioId = result.Value});
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
