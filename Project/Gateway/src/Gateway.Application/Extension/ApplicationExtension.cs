using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Gateway.Application.Extension
{
    public static class ApplicationExtension
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