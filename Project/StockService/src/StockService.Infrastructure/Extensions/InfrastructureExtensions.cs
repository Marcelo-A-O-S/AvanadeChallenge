using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace StockService.Infrastructure.Extensions
{
    public static class InfrastructureExtensions
    {
        public static IServiceCollection AddInfrastructureExtension(
            this IServiceCollection services, IConfiguration configuration
        )
        {
            services.AddDBContextExtension(configuration);
            services.AddRabbitMQConnection(configuration);
            services.AddDependencyInjection();
            services.AddHostedExtension();
            return services;
        }
    }
}