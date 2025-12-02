using Microsoft.EntityFrameworkCore;
using SaleService.Domain.Entities;
using SaleService.Domain.Enums;
using SaleService.Domain.Interfaces;
using SaleService.Infrastructure.Context;
namespace SaleService.Infrastructure.Repositories
{
    public class OrderRepository : Generics<Order>, IOrderRepository
    {
        private readonly DBContext context;
        public OrderRepository(DBContext _context) : base(_context)
        {
            this.context = _context;
        }

        public async Task<List<Order>> GetAllByUserId(long userId, int page = 1, int itemsPage = 10)
        {
            var query = this.context.Orders.AsQueryable();
            var items = await query.Where(o=> o.UserId == userId)
            .Include(o => o.Sales)
            .Skip((page - 1) * itemsPage)
            .Take(itemsPage)
            .ToListAsync();
            return items;
        }

        public async Task<int> GetQuantity()
        {
            return await this.context.Orders.CountAsync();
        }

        public async Task<int> GetQuantityByUserId(long userId)
        {
            return await this.context.Orders.Where(o => o.UserId == userId && o.Status == OrderStatus.CONFIRMED).CountAsync();
        }
    }
}