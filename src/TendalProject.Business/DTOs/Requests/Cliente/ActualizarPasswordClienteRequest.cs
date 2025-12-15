namespace TendalProject.Business.DTOs.Requests.Cliente
{
    public record ActualizarPasswordClienteRequest
    (
        Guid ClienteId,
        string PasswordActual,
        string NuevoPassword,
        string ConfirmarPassword
    );
}
