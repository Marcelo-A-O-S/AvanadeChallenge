using Microsoft.Extensions.DependencyInjection;
using SaleService.Infrastructure.Consumers;

namespace SaleService.Infrastructure.Extension
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