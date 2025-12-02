namespace Gateway.API.Extensions
{
    public static class CorsConfigExtension
    {
        public static IServiceCollection AddCorsConfig(
            this IServiceCollection services
        )
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.WithOrigins(
                            "http://localhost:3000",
                            "http://localhost:5000", 
                            "http://localhost:5001", 
                            "http://localhost:5002",
                            "http://localhost:5003"
                        )
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .WithExposedHeaders("Authorization");
                });
            });
            return services;
        }
    }
}