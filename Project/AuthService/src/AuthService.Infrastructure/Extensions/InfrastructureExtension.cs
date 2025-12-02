using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.Infrastructure.Extensions
{
    public static class InfrastructureExtension
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, IConfiguration configuration
        )
        {
            
            services.AddDBContextExtension(configuration);
            services.AddDependecyInjection();
            return services;
        }
    }
}