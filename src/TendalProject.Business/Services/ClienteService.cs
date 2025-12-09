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

        public async Task<Result<DetalleClienteResponse>> ObtenerDetalleClienteAsync(Guid clienteId)
        {
            var cliente = await _UoW.ClienteRepository.GetClienteWithUsuarioIdAsync(clienteId);
            if (cliente is null)
            {
                return Result<DetalleClienteResponse>.Failure(Error.NotFound("No se encontró el cliente con el ID proporcionado."));
            }
            var response = new DetalleClienteResponse
            (cliente.ClienteId,cliente.Nombre, cliente.ApellidoPaterno,
                cliente.ApellidoMaterno,
                cliente.CorreoElectronico,
                cliente.NumeroCelular!,
                cliente.FechaNacimiento,
                cliente.Estado.ToString(),
                cliente.Usuario?.UltimaConexion,
                cliente.FechaCreacion,
                cliente.FechaModificacion,
                cliente.Pedidos.Sum(p => p.Detalles.Sum(d => d.PrecioUnitario * d.Cantidad)),//NOTE: esto debemos cambiar
                cliente.Nivel.ToString(),
                cliente.CantidadPedidos
            );
            return Result<DetalleClienteResponse>.Success(response);
        }
    }
}
