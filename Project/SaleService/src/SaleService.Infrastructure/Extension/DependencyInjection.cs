using Microsoft.Extensions.DependencyInjection;
using SaleService.Domain.Entities;
using SaleService.Domain.Interfaces;
using SaleService.Infrastructure.Repositories;

namespace SaleService.Infrastructure.Extension
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
            return services;
        }
    }
}