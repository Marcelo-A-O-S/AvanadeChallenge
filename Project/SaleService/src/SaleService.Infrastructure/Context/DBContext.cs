using Microsoft.EntityFrameworkCore;
using SaleService.Domain.Entities;
namespace SaleService.Infrastructure.Context
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {

        }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Sale> Sales { get; set; }
    }
}