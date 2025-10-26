using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace SaleService.Infrastructure.Extension
{
    public static class InfrastructureExtensions
    {
        public static IServiceCollection AddInfrastructureExtensions(
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