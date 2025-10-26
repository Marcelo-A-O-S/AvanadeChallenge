using Microsoft.Extensions.DependencyInjection;
using SaleService.Application.Interfaces;
using SaleService.Application.Services;

namespace SaleService.Application.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencyInjections(
            this IServiceCollection services
        )
        {
            services.AddScoped<IOrderServices, OrderServices>();
            services.AddScoped<ISaleServices, SaleServices>();
            return services;
        }
    }
}