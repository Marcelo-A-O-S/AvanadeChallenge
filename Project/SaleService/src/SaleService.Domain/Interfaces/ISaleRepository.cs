using SaleService.Domain.Entities;
namespace SaleService.Domain.Interfaces
{
    public interface ISaleRepository: IGenerics<Sale>
    {
        Task<List<SalesInfoProduct>> GetSalesInfoProducts( int productInitial, int productFinally);
        Task<List<Sale>> GetByUserId(long userId,int page = 1, int itemsPage = 10);
        Task<List<SalesInfoProduct>> GetSalesInfoProductsByIds(List<long> productIds);
        Task<List<SalesInfoProduct>> GetSalesInfoProductsConfirmedByIds(List<long> productIds);
        Task AttachToOrder(long saleId, long orderId);
        Task<List<Sale>> GetByOrderId(long orderId);
        Task<List<Sale>> GetSalesInProgress(long userId,int page = 1, int itemsPage = 10);
        Task<int> GetQuantity();
        Task<int> GetQuantityInProgressByUserId(long userId);
    }
}