using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TendalProject.Business.Interfaces;
using TendalProject.Common.Results;
using TendalProject.Web.ViewModels.Auth;

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
            var result = await _usuarioService.RegistrarUsuarioAsync(new Business.DTOs.Requests.Usuario.RegistrarUsuarioRequest(
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
    }
}
