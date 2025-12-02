using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gateway.Application.Interfaces;
using Gateway.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Gateway.Application.Extension
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencyInjection(
            this IServiceCollection services
        )
        {
            
            services.AddScoped<ICatalogServices,CatalogServices>();
            services.AddScoped<ICartServices, CartServices>();
            return services;
        }
    }
}