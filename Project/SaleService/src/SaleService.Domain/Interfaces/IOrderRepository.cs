using SaleService.Domain.Entities;

namespace SaleService.Domain.Interfaces
{
    public interface IOrderRepository : IGenerics<Order>
    {
        Task<int> GetQuantity();
        Task<int> GetQuantityByUserId(long userId);
        Task<List<Order>> GetAllByUserId(long userId, int page = 1, int itemsPage = 10);
    }
}