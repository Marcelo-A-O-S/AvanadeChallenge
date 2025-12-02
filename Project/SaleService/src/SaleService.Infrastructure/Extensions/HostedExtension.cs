using Microsoft.Extensions.DependencyInjection;
using SaleService.Infrastructure.Messaging.Consumers;

namespace SaleService.Infrastructure.Extensions
{
    public static class HostedExtension
    {
        public static IServiceCollection AddHostedExtension(
            this IServiceCollection services
        )
        {
            services.AddHostedService<OrderConsumer>();
            services.AddHostedService<SaleConsumer>();
            return services;
        }
    }
}