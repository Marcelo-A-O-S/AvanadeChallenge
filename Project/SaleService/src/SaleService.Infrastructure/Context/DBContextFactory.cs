using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
namespace SaleService.Infrastructure.Context
{
    public class DBContextFactory : IDesignTimeDbContextFactory<DBContext>
    {
        public DBContext CreateDbContext(string[] args)
        {
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../SaleService.Api");
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = configuration.GetConnectionString("ConnectionString");

            var optionsBuilder = new DbContextOptionsBuilder<DBContext>();
            optionsBuilder.UseNpgsql(connectionString);
            return new DBContext(optionsBuilder.Options);
        }
    }
}