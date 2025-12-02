using Microsoft.Extensions.DependencyInjection;
using AuthService.Domain.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Infrastructure.Repositories;
namespace AuthService.Infrastructure.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddDependecyInjection(
            this IServiceCollection services
        )
        {
            services.AddScoped<IGenerics<User>, Generics<User>>();

            services.AddScoped<IUserRepository, UserRepository>(); 
            return services;
        }
    }
}