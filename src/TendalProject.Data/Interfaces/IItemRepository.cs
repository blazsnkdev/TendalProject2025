using TendalProject.Entities.Entidades;

namespace TendalProject.Data.Interfaces
{
    public interface IItemRepository : IRepository<Item>
    {
        void EliminarItem(Item item);
    }
}
