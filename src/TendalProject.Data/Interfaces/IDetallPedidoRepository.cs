using TendalProject.Entities.Entidades;

namespace TendalProject.Data.Interfaces
{
    public interface IDetallPedidoRepository : IRepository<DetallePedido>
    {
        Task<List<DetallePedido>> GetDetallesIncludArticuloByPedidoId(Guid pedidoId);
    }
}
