namespace TendalProject.Business.DTOs.Requests.Auth
{
    public record CredencialesLoginRequest
    (
        string Email,
        string Password
    );
}
