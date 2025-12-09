using TendalProject.Business.DTOs.Responses.Cliente;
using TendalProject.Business.Interfaces;
using TendalProject.Common.Results;
using TendalProject.Data.UnitOfWork;
using TendalProject.Entities.Entidades;

namespace TendalProject.Business.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IUnitOfWork _UoW;

        public ClienteService(IUnitOfWork uoW)
        {
            _UoW = uoW;
        }

        public async Task<Result<List<ListarClienteResponse>>> ListarClientesAsync(string? nombre)
        {
            var clientes = nombre is null
                ? await _UoW.ClienteRepository.GetAllAsync()
                : await _UoW.ClienteRepository.GetAllClientesByNombre(nombre);
            if(clientes is null || !clientes.Any())
            {
                return Result<List<ListarClienteResponse>>.Success(new List<ListarClienteResponse>());
            }
            var response = clientes.Select(c => new ListarClienteResponse(
                    c.ClienteId,
                    c.Nombre,
                    $"{c.ApellidoPaterno} {c.ApellidoMaterno}",
                    c.CorreoElectronico,
                    c.NumeroCelular!,
                    c.Estado.ToString()
                    )).ToList();
            return Result<List<ListarClienteResponse>>.Success(response);
        }
    }
}
