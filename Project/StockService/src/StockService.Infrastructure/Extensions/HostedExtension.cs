using Microsoft.Extensions.DependencyInjection;
using StockService.Infrastructure.Messaging.Consumers;

namespace StockService.Infrastructure.Extensions
{
    public static class HostedExtension
    {
        public static IServiceCollection AddHostedExtension(
            this IServiceCollection services
        )
        {
            services.AddHostedService<ProductConsumer>();
            services.AddHostedService<StockMovementConsumer>();
            return services;
        }
    }
}