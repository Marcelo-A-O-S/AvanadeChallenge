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
        public async Task<Product> GetByName(string name)
        {
            return await this.context.Products.Where(x => x.Name == name).FirstOrDefaultAsync();
        }
    }
}