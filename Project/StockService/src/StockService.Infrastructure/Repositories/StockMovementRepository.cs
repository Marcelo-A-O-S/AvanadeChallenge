using Microsoft.EntityFrameworkCore;
using StockService.Domain.Entities;
using StockService.Domain.Interfaces;
using StockService.Infrastructure.Context;
namespace StockService.Infrastructure.Repositories
{
    public class StockMovementRepository : Generics<StockMovement>, IStockMovementRepository
    {
        private readonly DBContext context;
        public StockMovementRepository(DBContext _context) : base(_context)
        {
            this.context = _context;
        }

        public async Task<List<StockMovement>> GetAllByOrderId(long orderId)
        {
            return await this.context.StockMovements.Where(x => x.OrderId == orderId).ToListAsync();
        }

        public async Task<List<StockMovement>> GetByIds(List<long> ids)
        {
            return await this.context.StockMovements.Where(s => ids.Contains(s.Id)).ToListAsync();
        }

        public async Task<StockMovement> GetByOrderId(long orderId)
        {
            return await this.context.StockMovements.Where(x => x.OrderId == orderId).FirstOrDefaultAsync();
        }

        public async Task<int> GetQuantity()
        {
            return await this.context.StockMovements.CountAsync();
        }
    }
}