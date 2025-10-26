using Microsoft.Extensions.DependencyInjection;
using StockService.Domain.Entities;
using StockService.Domain.Interfaces;
using StockService.Infrastructure.Repositories;

namespace StockService.Infrastructure.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencyInjection(
            this IServiceCollection services
        )
        {
            //Generics
            services.AddScoped<IGenerics<Product>, Generics<Product>>();
            services.AddScoped<IGenerics<StockMovement>, Generics<StockMovement>>();
            //Repositories
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IStockMovementRepository, StockMovementRepository>();
            return services;
        }
    }
}