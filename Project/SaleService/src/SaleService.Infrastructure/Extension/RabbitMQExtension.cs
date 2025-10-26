using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using Microsoft.Extensions.Configuration;

namespace SaleService.Infrastructure.Extension
{
    public static class RabbitMQExtension
    {
        public static IServiceCollection AddRabbitMQConnection(
            this IServiceCollection services, IConfiguration configuration
        )
        {
            services.AddSingleton<IConnectionFactory>(_ =>
            {
                var host = configuration.GetValue<string>("RabbitMQ:Host") ?? "rabbitmq";
                var username = configuration.GetValue<string>("RabbitMQ:UserName") ?? "guest";
                var password = configuration.GetValue<string>("RabbitMQ:Password") ?? "guest";
                return new ConnectionFactory()
                {
                    HostName = host,
                    UserName = username,
                    Password = password
                };
            });
            return services;
        }
    }
}