using Microsoft.EntityFrameworkCore;
using StockService.Domain.Entities;
namespace StockService.Infrastructure.Context
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<StockMovement> StockMovements { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StockMovement>().Property(p=> p.Type).HasConversion<string>();
            modelBuilder.Entity<StockMovement>().Property(p=> p.Reason).HasConversion<string>();
        }
    }
}