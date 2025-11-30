using Microsoft.AspNetCore.Mvc;
using TendalProject.Business.DTOs.Requests;
using TendalProject.Business.Interfaeces;
using TendalProject.Web.ViewModels;

namespace TendalProject.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        public IActionResult Login()=> View();
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            var result = await _authService.LoginAsync(new CredencialesLoginRequest(Email:viewModel.Email,Password:viewModel.Password));
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, "Credenciales inválidas.");
                return View(viewModel);
            }
            if(result.Value is null)
            {
                ModelState.AddModelError(string.Empty, "Error inesperado durante el inicio de sesión.");
                return View(viewModel);
            }
            await _authService.SignInAsync(HttpContext, result.Value, viewModel.Recordarme);
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Registro()=> View();
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(RegistrarUsuarioViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var result = await _authService.RegistroAsync(new RegistroUsuarioRequest(
                viewModel.Nombre,
                viewModel.ApellidoPaterno,
                viewModel.ApellidoMaterno,
                viewModel.NumeroCelular,
                viewModel.FechaNacimiento,
                viewModel.Email,
                viewModel.Password,
                viewModel.ConfirmarPassword));

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, "Error al registrar el usuario.");
                return View(viewModel);
            }

            return RedirectToAction("Login", "Auth");
        }   
    }
}
