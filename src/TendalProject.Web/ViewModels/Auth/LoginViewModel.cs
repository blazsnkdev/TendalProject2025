namespace TendalProject.Web.ViewModels.Auth
{
    public class LoginViewModel
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool Recordarme { get; set; }
    }
}
