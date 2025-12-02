using SaleService.Domain.Entities;
namespace SaleService.Application.Interfaces
{
    public interface IOrderServices: IServices<Order>
    {
        Task<int> GetQuantity();
        Task<int> GetQuantityByUserId(long userId);
        Task<List<Order>> GetAllByUserId(long userId, int page = 1, int itemsPage = 10);
    }
}