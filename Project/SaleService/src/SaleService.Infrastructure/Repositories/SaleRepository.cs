using SaleService.Domain.Entities;
using SaleService.Domain.Interfaces;
using SaleService.Infrastructure.Context;
namespace SaleService.Infrastructure.Repositories
{
    public class SaleRepository : Generics<Sale>, ISaleRepository
    {
        public SaleRepository(DBContext _context) : base(_context)
        {
        }
    }
}