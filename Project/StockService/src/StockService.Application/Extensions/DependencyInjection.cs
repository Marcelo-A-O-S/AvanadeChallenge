
using Microsoft.Extensions.DependencyInjection;
using StockService.Application.Interfaces;
using StockService.Application.Services;
using StockService.Domain.Entities;

namespace StockService.Application.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencyInjection(
            this IServiceCollection services
        )
        {
            
            services.AddScoped<IProductServices, ProductServices>();
            services.AddScoped<IStockMovementServices, StockMovementServices>();
            return services;
        }
    }
}