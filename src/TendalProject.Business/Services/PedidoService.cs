using Microsoft.EntityFrameworkCore;
using TendalProject.Business.DTOs.Responses.Pedido;
using TendalProject.Business.Interfaces;
using TendalProject.Common.Results;
using TendalProject.Data.UnitOfWork;

namespace TendalProject.Business.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IUnitOfWork _UoW;

        public PedidoService(IUnitOfWork uoW)
        {
            _UoW = uoW;
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

    }
}
