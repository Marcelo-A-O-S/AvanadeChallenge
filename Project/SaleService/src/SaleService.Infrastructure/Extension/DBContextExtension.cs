using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SaleService.Infrastructure.Context;

namespace SaleService.Infrastructure.Extension
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