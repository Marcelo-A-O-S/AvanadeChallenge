using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
namespace StockService.API.Extensions
{
    public static class JwtConfigExtension
    {
        public static IServiceCollection AddJwtAuthentication(
            this IServiceCollection services, IConfiguration configuration
        )
        {
            var key = configuration.GetSection("JWT:Key").Value;
            if (string.IsNullOrEmpty(key))
            {
                throw new InvalidOperationException("JWT Secret não configurado.");
            }
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                };
            });
            return services;
        }
    }
}