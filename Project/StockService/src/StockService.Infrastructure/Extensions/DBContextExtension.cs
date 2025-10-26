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
            services.AddDbContext<DBContext>(options =>
            {
                options.UseNpgsql("");
            });
            return services;
        }
    }
}