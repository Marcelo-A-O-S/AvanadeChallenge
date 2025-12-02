using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.API.Extensions
{
    public static class AuthorizationConfig
    {
        public static IServiceCollection AddAuthorizationConfig(
         this IServiceCollection services
        )
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Administrador", policy =>
                {
                    policy.RequireRole("Administrador");
                });
                options.AddPolicy("Client", policy =>
                {
                    policy.RequireRole("Client");
                });
                options.AddPolicy("Employee", policy =>
                {
                    policy.RequireRole("Employee");
                });
            });
            return services;
        }
    }
}