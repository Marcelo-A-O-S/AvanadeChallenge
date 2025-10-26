using StockService.Domain.Entities;
namespace StockService.Domain.Interfaces
{
    public interface IProductRepository : IGenerics<Product>
    {
        Task<Product> GetByName(string name);
    }
}