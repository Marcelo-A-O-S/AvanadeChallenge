using Microsoft.Extensions.DependencyInjection;

namespace StockService.Application.Extensions
{
    public static class ApplicationLayerExtension
    {
        public static IServiceCollection AddApplicationLayerExtension(
            this IServiceCollection services
        )
        {
            services.AddDependencyInjections();
            return services;
        }
    }
}