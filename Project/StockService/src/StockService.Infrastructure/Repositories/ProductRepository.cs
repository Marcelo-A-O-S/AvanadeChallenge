using Microsoft.EntityFrameworkCore;
using StockService.Domain.Entities;
using StockService.Domain.Interfaces;
using StockService.Infrastructure.Context;
namespace StockService.Infrastructure.Repositories
{
    public class ProductRepository : Generics<Product>, IProductRepository
    {
        private readonly DBContext context;
        public ProductRepository(DBContext _context) : base(_context)
        {
            this.context = _context;
        }

        public async Task<List<Product>> GetByIds(List<long> ids)
        {
            return await this.context.Products.Where(p => ids.Contains(p.Id)).ToListAsync();
        }
        public async Task<List<Product>> GetProductsWithStock(int page = 1, int itemsPage = 10)
        {
            var query = this.context.Products.AsQueryable();
            var items = await query.Where(p=> p.Quantity > 0).Skip((page - 1) * itemsPage)
            .Take(itemsPage)
            .ToListAsync();
            return items;
        }
        public async Task<Product> GetByName(string name)
        {
            return await this.context.Products.Where(x => x.Name == name).FirstOrDefaultAsync();
        }

        public async Task<List<Product>> GetProductsWithStock()
        {
            return await this.context.Products.Where(p=> p.Quantity > 0).ToListAsync();
        }
        public async Task<int> GetQuantity()
        {
            return await this.context.Products.CountAsync();
        }
    }
}