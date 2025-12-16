using TendalProject.Business.DTOs.Responses.Dashboard;
using TendalProject.Business.Interfaces;
using TendalProject.Common.Results;
using TendalProject.Data.UnitOfWork;

namespace TendalProject.Business.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _UoW;
        public DashboardService(IUnitOfWork uoW)
        {
            _UoW = uoW;
        }
        public async Task<Result<DatosDashboardResponse>> GetDatosCabeceraDashboardAsync()
        {
            var ventasHoy = await _UoW.VentaRepository.GetVentasHoyAsync();
            var ventasMes = await _UoW.VentaRepository.GetVentasMesAsync();
            var pedidosHoy = await _UoW.PedidoRepository.GetPedidosHoyAsync();
            var pedidosPendientes = await _UoW.PedidoRepository.GetPedidosPendientesAsync();

            var response = new DatosDashboardResponse(
                ventasHoy,
                ventasMes,
                pedidosHoy,
                pedidosPendientes
            );

            return Result<DatosDashboardResponse>.Success(response);
        }


    }
}
