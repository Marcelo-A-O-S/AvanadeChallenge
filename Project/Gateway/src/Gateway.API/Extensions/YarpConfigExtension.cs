using Gateway.API.Configuration;
using System.Security.Cryptography.Xml;
using Yarp.ReverseProxy.Transforms;

namespace Gateway.API.Extensions
{
    public static class YarpConfigExtension
    {
        public static IServiceCollection AddYarpConfig(
            this IServiceCollection services
        )
        {
            services.AddReverseProxy()
            .LoadFromMemory(BasicConfiguration.GetRoutes(),BasicConfiguration.GetClusters());
            return services;
        }
    }
}