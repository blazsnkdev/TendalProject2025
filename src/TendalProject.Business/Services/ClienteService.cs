using TendalProject.Business.DTOs.Responses.Cliente;
using TendalProject.Business.Interfaces;
using TendalProject.Common.Results;
using TendalProject.Data.UnitOfWork;
using TendalProject.Entities.Entidades;
using TendalProject.Entities.Enum;

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
            if(clienteId == Guid.Empty)
            {
                return Result<DetalleClienteResponse>.Failure(Error.NotFound("ID de cliente no válido."));
            }
            var cliente = await _UoW.ClienteRepository.GetClienteWithUsuarioIdAsync(clienteId);
            if (cliente is null)
            {
                return Result<DetalleClienteResponse>.Failure(Error.NotFound("No se encontró el cliente con el ID proporcionado."));
            }
            var importeTotal = await _UoW.VentaRepository.GetVentasPorClienteIdAsync(clienteId);
            var cantidadCompras = await _UoW.VentaRepository.GetCantidadVentasPorClienteIdAsync(clienteId);
            var pedidos = await _UoW.PedidoRepository.GetPedidosPorClienteAsync(clienteId);
            var cantidadPedidosPendientes = pedidos.Count(p => p.Estado == EstadoPedido.Pendiente);
            var cantidadPedidosProcesados = pedidos.Count(p => p.Estado == EstadoPedido.Procesando);
            var cantidadPedidosEnviados = pedidos.Count(p => p.Estado == EstadoPedido.Enviado);
            var cantidadPedidosEntregados = pedidos.Count(p => p.Estado == EstadoPedido.Entregado);
            var cantidadPedidosCancelados = pedidos.Count(p => p.Estado == EstadoPedido.Cancelado);


            var response = new DetalleClienteResponse
            (cliente.ClienteId, cliente.Nombre, cliente.ApellidoPaterno,
                cliente.ApellidoMaterno,
                cliente.CorreoElectronico,
                cliente.NumeroCelular ?? string.Empty,
                cliente.FechaNacimiento,
                cliente.Estado.ToString(),
                cliente.Usuario?.UltimaConexion,
                cliente.FechaCreacion,
                cliente.FechaModificacion,
                importeTotal,
                cliente.Nivel.ToString(),
                cantidadCompras,
                cantidadPedidosPendientes,
                cantidadPedidosProcesados,
                cantidadPedidosEnviados,
                cantidadPedidosEntregados,
                cantidadPedidosCancelados
            );
            return Result<DetalleClienteResponse>.Success(response);
        }

        public async Task<Result<List<ListaPedidosPorClienteResponse>>> ObtenerPedidosPorClienteAsync(Guid clienteId)
        {
            if(clienteId == Guid.Empty)
            {
                return Result<List<ListaPedidosPorClienteResponse>>.Failure(Error.NotFound("ID de cliente no válido."));
            }
            var pedidos = await _UoW.PedidoRepository.GetPedidosPorClienteAsync(clienteId);
            if (pedidos is null || !pedidos.Any())
            {
                return Result<List<ListaPedidosPorClienteResponse>>.Success(new List<ListaPedidosPorClienteResponse>());
            }
            var response = new List<ListaPedidosPorClienteResponse>(
                pedidos.Select(p => new ListaPedidosPorClienteResponse  
                (
                    p.PedidoId,
                    p.Codigo,
                    p.FechaRegistro,
                    p.Total,
                    p.Estado.ToString()
                ))
            );
            return Result<List<ListaPedidosPorClienteResponse>>.Success(response);
        }
    }
}
