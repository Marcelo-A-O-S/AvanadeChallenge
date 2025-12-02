using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.Application.Extensions
{
    public static class ApplicationLayerExtensions
    {
        public static IServiceCollection AddApplicationExtensions(
            this IServiceCollection services
        )
        {
            services.AddDependencyInjection();
            return services;
        }
    }
}