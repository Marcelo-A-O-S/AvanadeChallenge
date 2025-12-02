using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SaleService.Infrastructure.Context;

namespace SaleService.Infrastructure.Extensions
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