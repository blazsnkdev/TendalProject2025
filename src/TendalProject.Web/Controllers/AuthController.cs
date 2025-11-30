using Azure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TendalProject.Business.DTOs.Requests;
using TendalProject.Business.DTOs.Responses;
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
            var result = await _authService.LoginAsync(new CredencialesLoginDto(Email:viewModel.Email,Password:viewModel.Password));
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
    }
}
