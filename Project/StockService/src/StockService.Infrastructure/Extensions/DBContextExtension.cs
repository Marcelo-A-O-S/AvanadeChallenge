using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using StockService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
namespace StockService.Infrastructure.Extensions
{
    public static class DBContextExtension
    {
        public static IServiceCollection AddDBContextExtension(
            this IServiceCollection services, IConfiguration configuration
        )
        {
            var connectionString = configuration.GetConnectionString("ConnectionString");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("ConnectionString não configurada.");
            }
            services.AddDbContext<DBContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });
            return services;
        }
    }
}