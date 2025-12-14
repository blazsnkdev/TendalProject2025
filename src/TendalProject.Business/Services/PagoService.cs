using ***REMOVED***
using ***REMOVED***
using ***REMOVED***
using Microsoft.Extensions.Configuration;
using TendalProject.Business.Interfaces;
using TendalProject.Common.Results;
using TendalProject.Common.Time;
using TendalProject.Data.UnitOfWork;
using TendalProject.Entities.Entidades;
using TendalProject.Entities.Enum;

namespace TendalProject.Business.Services
{
    public class PagoService : IPagoService
    {
        private readonly IUnitOfWork _UoW;
        private readonly IDateTimeProvider _dateTimeProvider;
        public PagoService(
            IConfiguration config,
            IUnitOfWork UoW,
            IDateTimeProvider dateTimeProvider)
        {
            var token = config["***REMOVED***
            if (string.IsNullOrEmpty(token))
                throw new Exception("¡No se encontró el ***REMOVED***

            ***REMOVED***
            _UoW = UoW;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Result<string>> CrearPrefernciaPagoAsync(Guid clienteId)
        {
            var carrito = await _UoW.CarritoRepository.GetCarritoByClienteIdAsync(clienteId);
            if (carrito == null || !carrito.Items.Any())
            {
                return Result<string>.Failure(Error.NotFound("El carrito está vacío o no existe."));
            }
            var s = ***REMOVED***
            var items = carrito.Items.Select(i => new PreferenceItemRequest
            {
                Title = i.Articulo.Nombre,
                Quantity = i.Cantidad,
                UnitPrice = i.PrecioFinal
            }).ToList();

            var request = new PreferenceRequest
            {
                Items = items,
                Payer = new PreferencePayerRequest
                {
                    Email = carrito.Cliente.CorreoElectronico 
                },
                BackUrls = new PreferenceBackUrlsRequest
                {
                    Success = "https://nonstimulating-semestral-frankie.ngrok-free.dev/Pago/PagoExitoso",
                    Failure = "https://nonstimulating-semestral-frankie.ngrok-free.dev/Ecommerce/Carrito",
                    Pending = "https://nonstimulating-semestral-frankie.ngrok-free.dev/Ecommerce/Carrito"
                },
                AutoReturn = "approved"
            };

            var client = new PreferenceClient();
            var preference = await client.CreateAsync(request);

            return Result<string>.Success(preference.InitPoint); 
        }

        public async Task<Result> ProcesarPagoExitosoAsync(Guid clienteId, string paymentId)
        {
            var carrito = await _UoW.CarritoRepository.GetCarritoByClienteIdAsync(clienteId);

            var subtotal = carrito.Items.Sum(i => i.PrecioFinal);
            var igv = subtotal * 0.18m;

            var pedido = new Pedido
            {
                PedidoId = Guid.NewGuid(),
                ClienteId = clienteId,
                Codigo = $"P{_dateTimeProvider.GetDateTimeNow()}",
                FechaRegistro = DateTime.Now,
                FechaPago = DateTime.Now,
                SubTotal = subtotal,
                Igv = igv,
                Total = subtotal + igv,
                Estado = EstadoPedido.Cancelado
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

            var venta = new Venta
            {
                VentaId = Guid.NewGuid(),
                Pedido = pedido,
                FechaVenta = DateTime.Now,
                MetodoPago = MetodoPago.***REMOVED***
                TipoComprobante = TipoComprobante.Boleta,
                NumeroComprobante = $"B-{DateTime.Now.Ticks}"
            };

            await _UoW.PedidoRepository.AddAsync(pedido);
            await _UoW.VentaRepository.AddAsync(venta);

            carrito.Items.Clear();

            await _UoW.SaveChangesAsync();
            return Result.Success();
        }
    }
}
