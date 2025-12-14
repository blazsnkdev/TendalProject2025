using Microsoft.EntityFrameworkCore;
using TendalProject.Business.DTOs.Requests.Pedido;
using TendalProject.Business.DTOs.Responses.Pedido;
using TendalProject.Business.Interfaces;
using TendalProject.Common.Results;
using TendalProject.Common.Time;
using TendalProject.Data.UnitOfWork;
using TendalProject.Entities.Entidades;
using TendalProject.Entities.Enum;

namespace TendalProject.Business.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IUnitOfWork _UoW;
        private readonly IDateTimeProvider _dateTimeProvider;

        public PedidoService(
            IUnitOfWork uoW,
            IDateTimeProvider dateTimeProvider
            )
        {
            _UoW = uoW;
            _dateTimeProvider = dateTimeProvider;
        }
        public async Task<Result<List<ListaPedidosResponse>>> ObtenerPedidosAsync(
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null,
            string? codigo = null,
            bool? proximosAEntregar = null,
            string? ordenarPor = "fecha",       
            string? orden = "desc"              
        )
        {
            var query = _UoW.PedidoRepository.GetPedidosIncludsAsync();
            if (fechaInicio is not null)
                query = query.Where(p => p.FechaRegistro >= fechaInicio);

            if (fechaFin is not null)
                query = query.Where(p => p.FechaRegistro <= fechaFin);
            if (!string.IsNullOrWhiteSpace(codigo))
                query = query.Where(p => p.Codigo.Contains(codigo));
            if (proximosAEntregar == true)
            {
                var hoy = DateTime.UtcNow.Date;
                var manana = hoy.AddDays(1);

                query = query.Where(p =>
                    p.FechaEntrega != null &&
                    p.FechaEntrega.Value.Date >= hoy &&
                    p.FechaEntrega.Value.Date <= manana
                );
            }

            query = (ordenarPor, orden.ToLower()) switch
            {
                ("fecha", "asc") => query.OrderBy(p => p.FechaRegistro),
                ("fecha", "desc") => query.OrderByDescending(p => p.FechaRegistro),

                ("entrega", "asc") => query.OrderBy(p => p.FechaEntrega),
                ("entrega", "desc") => query.OrderByDescending(p => p.FechaEntrega),

                _ => query.OrderByDescending(p => p.FechaRegistro) // default
            };

            var pedidos = await query.ToListAsync();
            var listaPedidos = pedidos.Select(p => new ListaPedidosResponse(
                p.PedidoId,
                p.Codigo,
                p.Cliente!.Nombre,
                p.FechaRegistro,
                p.FechaEntrega,
                p.Total,
                p.Estado.ToString(),
                p.Detalles.Count
            )).ToList();

            return Result<List<ListaPedidosResponse>>.Success(listaPedidos);
        }

        public async Task<Result<List<HistorialPedidosClienteResponse>>> ObtenerPedidosPorClienteAsync(Guid clienteId)
        {
            if(clienteId == Guid.Empty)
            {
                return Result<List<HistorialPedidosClienteResponse>>.Failure(Error.Validation("ClienteId es invalido"));
            }
            var pedidos = await _UoW.PedidoRepository.GetPedidosIncludsPorClienteAsync(clienteId);
            var response = new List<HistorialPedidosClienteResponse>();
            if(!pedidos.Any())
            {
                return Result<List<HistorialPedidosClienteResponse>>.Success(response);
            }
            response = pedidos.Select(p => new HistorialPedidosClienteResponse(
                p.PedidoId,
                p.Codigo,
                p.Total,
                p.FechaRegistro,
                p.FechaEntrega.HasValue ? DateOnly.FromDateTime(p.FechaEntrega.Value) : null,
                p.Estado.ToString(),
                p.Detalles.Count()
                )).ToList();
            return Result<List<HistorialPedidosClienteResponse>>.Success(response);
        }

        public async Task<Result<string>> GenerarCodigoPedidoAsync()
        {
            var pedidos = await _UoW.PedidoRepository.GetAllAsync();
            if (pedidos is null || !pedidos.Any())
            {
                return Result<string>.Success("PED-00000001");
            }
            else
            {
                var ultimoPedido = pedidos
                    .OrderByDescending(p => p.FechaRegistro)
                    .FirstOrDefault();
                if (ultimoPedido is null)
                {
                    return Result<string>.Success("PED-00000001");
                }
                var ultimoCodigo = ultimoPedido.Codigo;
                var numeroUltimoCodigo = int.Parse(ultimoCodigo.Split('-')[1]);
                var nuevoNumeroCodigo = numeroUltimoCodigo + 1;
                var nuevoCodigo = $"PED-{nuevoNumeroCodigo.ToString("D8")}";
                return Result<string>.Success(nuevoCodigo);
            }
        }

        public async Task<Result<Pedido>> CrearPedidoPendienteAsync(CrearPedidoPendienteRequest request)
        {
            var carrito = await _UoW.CarritoRepository.GetCarritoByClienteIdAsync(request.ClienteId);
            if (!carrito.Items.Any())
            {
                return Result<Pedido>.Failure(Error.NotFound("No se encontraron articulos agregados"));
            }
            var subtotal = carrito.Items.Sum(i => i.PrecioFinal);
            var igv = subtotal * 0.18m;
            var codigoPedido = await GenerarCodigoPedidoAsync();

            var pedido = new Pedido
            {
                PedidoId = Guid.NewGuid(),
                ClienteId = request.ClienteId,
                Codigo = codigoPedido.Value!,
                FechaRegistro = _dateTimeProvider.GetDateTimeNow(),
                FechaEntrega = request.FechaEntrega,
                SubTotal = subtotal,
                Igv = igv,
                Total = subtotal + igv,
                Estado = EstadoPedido.Pendiente
            };

            foreach (var item in carrito.Items)
            {
                pedido.Detalles.Add(new DetallePedido
                {
                    DetallePedidoId = Guid.NewGuid(),
                    ArticuloId = item.ArticuloId,
                    Cantidad = item.Cantidad,
                    PrecioUnitario = item.PrecioFinal
                });
            }

            await _UoW.PedidoRepository.AddAsync(pedido);
            await _UoW.SaveChangesAsync();

            return Result<Pedido>.Success(pedido);
        }
    }
}
