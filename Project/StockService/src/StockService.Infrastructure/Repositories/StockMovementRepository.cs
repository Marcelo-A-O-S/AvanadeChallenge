using StockService.Domain.Entities;
using StockService.Domain.Interfaces;
using StockService.Infrastructure.Context;
namespace StockService.Infrastructure.Repositories
{
    public class StockMovementRepository : Generics<StockMovement>, IStockMovementRepository
    {
        public StockMovementRepository(DBContext _context) : base(_context)
        {
        }
    }
}