using Azure.Core;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Resource.Preference;
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
        private readonly IPedidoService _pedidoService;
        public PagoService(
            IConfiguration config,
            IUnitOfWork UoW,
            IDateTimeProvider dateTimeProvider,
            IPedidoService pedidoService
            )
        {
            var token = config["MercadoPago:AccessToken"];
            if (string.IsNullOrEmpty(token))
                throw new Exception("¡No se encontró el AccessToken de MercadoPago!");
            MercadoPagoConfig.AccessToken = token;
            _UoW = UoW;
            _dateTimeProvider = dateTimeProvider;
            _pedidoService = pedidoService;
        }

        public async Task<Result<string>> CrearPrefernciaPagoAsync(Guid pedidoId)
        {
            var pedido = await _UoW.PedidoRepository.GetByIdAsync(pedidoId);//ojito
            if (pedido == null || !pedido.Detalles.Any())
                return Result<string>.Failure(Error.NotFound("Pedido no encontrado"));

            var items = pedido.Detalles.Select(d => new PreferenceItemRequest
            {
                Title = d.Articulo.Nombre,
                Quantity = d.Cantidad,
                UnitPrice = d.PrecioUnitario
            }).ToList();

            var preferenceRequest = new PreferenceRequest
            {
                Items = items,
                Payer = new PreferencePayerRequest
                {
                    Email = pedido.Cliente.CorreoElectronico
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
            var preference = await client.CreateAsync(preferenceRequest);

            return Result<string>.Success(preference.InitPoint);
        }

        public async Task<Result> ProcesarPagoExitosoAsync(Guid clienteId, string paymentId)
        {
            var pedido = await _UoW.PedidoRepository.GetPedidoPendienteByClienteIdAsync(clienteId);
            if (pedido == null)
                return Result.Failure(Error.NotFound("Pedido pendiente no encontrado"));

            await _UoW.BeginTransactionAsync();
            try
            {
                pedido.Estado = EstadoPedido.Pagado;
                pedido.FechaPago = _dateTimeProvider.GetDateTimeNow();
                foreach (var detalle in pedido.Detalles)
                {
                    var articulo = await _UoW.ArticuloRepository.GetByIdAsync(detalle.ArticuloId);
                    if (articulo is null)
                    {
                        return Result.Failure(Error.NotFound("Artículo no encontrado"));
                    }
                    if (articulo.Stock < detalle.Cantidad)
                    {
                        return Result.Failure(Error.Validation("Stock insuficiente"));
                    }
                    articulo.Stock -= detalle.Cantidad;
                    articulo.CantidadVentas += detalle.Cantidad;
                }
                var venta = new Venta
                {
                    VentaId = Guid.NewGuid(),
                    Pedido = pedido,
                    FechaVenta = _dateTimeProvider.GetDateTimeNow(),
                    MetodoPago = MetodoPago.MercadoPago,
                    TipoComprobante = TipoComprobante.Boleta,
                    NumeroComprobante = $"B-{_dateTimeProvider.GetDateTimeNow().Ticks}" 
                };

                await _UoW.VentaRepository.AddAsync(venta);
                var carrito = await _UoW.CarritoRepository.GetCarritoByClienteIdAsync(clienteId);
                carrito.Items.Clear();
                await _UoW.CommitTransactionAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                await _UoW.RollBackAsync();
                return Result.Failure(Error.Internal($"Error procesando el pago: {ex.Message}"));
            }
        }


    }
}
