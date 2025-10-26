using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace StockService.Infrastructure.Extensions
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