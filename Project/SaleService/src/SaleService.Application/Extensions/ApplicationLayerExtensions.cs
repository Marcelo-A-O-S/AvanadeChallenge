using Microsoft.Extensions.DependencyInjection;

namespace SaleService.Application.Extensions
{
    public static class ApplicationLayerExtensions
    {
        public static IServiceCollection AddApplicationLayerExtensions(
            this IServiceCollection services
        )
        {
            services.AddDependencyInjections();
            return services;
        }
    }
}