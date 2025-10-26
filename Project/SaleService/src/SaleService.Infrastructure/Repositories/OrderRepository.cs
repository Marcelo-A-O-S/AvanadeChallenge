using SaleService.Domain.Entities;
using SaleService.Domain.Interfaces;
using SaleService.Infrastructure.Context;
namespace SaleService.Infrastructure.Repositories
{
    public class OrderRepository : Generics<Order>, IOrderRepository
    {
        public OrderRepository(DBContext _context) : base(_context)
        {
        }
    }
}