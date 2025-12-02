using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SaleService.Infrastructure.Context;

namespace SaleService.Infrastructure.Extensions
{
    public static class DBInitializeExtension
    {
        public static IServiceProvider ApplyMigrations(
            this IServiceProvider services
        )
        {
            using(var scope = services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var context = serviceProvider.GetRequiredService<DBContext>();
                try
                {
                    context.Database.Migrate();
                    
                }catch(Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            return services;
        }
    }
}