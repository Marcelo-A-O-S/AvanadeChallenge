using StockService.Domain.Entities;
namespace StockService.Domain.Interfaces
{
    public interface IStockMovementRepository : IGenerics<StockMovement>
    {
        Task<StockMovement> GetByOrderId(long orderId);
        Task<List<StockMovement>> GetAllByOrderId(long orderId);
        Task<List<StockMovement>> GetByIds(List<long> ids);
        Task<int> GetQuantity();
    }
}