using Microsoft.Extensions.DependencyInjection;
using SaleService.Domain.Entities;
using SaleService.Domain.Interfaces;
using SaleService.Infrastructure.Context;
using SaleService.Infrastructure.Repositories;
using SaleService.Infrastructure.Workers;

namespace SaleService.Infrastructure.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencyInjection(
            this IServiceCollection services
        )
        {
            //Generics
            services.AddScoped<IGenerics<Order>, Generics<Order>>();
            services.AddScoped<IGenerics<Sale>, Generics<Sale>>();
            //Repositories
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ISaleRepository, SaleRepository>();

            services.AddSingleton<IRabbitMQProducer, RabbitMQProducer>();
            return services;
        }
    }
}