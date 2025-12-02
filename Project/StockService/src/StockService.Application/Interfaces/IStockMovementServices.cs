using StockService.Domain.Entities;
namespace StockService.Application.Interfaces
{
    public interface IStockMovementServices : IServices<StockMovement>
    {
        Task<StockMovement> GetByOrderId(long orderId);
        Task<List<StockMovement>> GetAllByOrderId(long orderId);
        Task<List<StockMovement>> GetByIds(List<long> ids);
        Task<int> GetQuantity();
    }
}