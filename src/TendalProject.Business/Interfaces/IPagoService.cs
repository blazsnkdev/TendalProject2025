using TendalProject.Common.Results;

namespace TendalProject.Business.Interfaces
{
    public interface IPagoService
    {
        Task<Result<string>> CrearPrefernciaPagoAsync(Guid pedidoId);
        Task<Result> ProcesarPagoExitosoAsync(Guid clienteId, string paymentId);
    }
}
