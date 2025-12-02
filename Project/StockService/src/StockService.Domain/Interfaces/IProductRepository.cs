using StockService.Domain.Entities;
namespace StockService.Domain.Interfaces
{
    public interface IProductRepository : IGenerics<Product>
    {
        Task<Product> GetByName(string name);
        Task<List<Product>> GetProductsWithStock();
        Task<List<Product>> GetProductsWithStock(int page = 1, int itemsPage = 10);
        Task<List<Product>> GetByIds(List<long> ids);

        Task<int> GetQuantity();
    }
}