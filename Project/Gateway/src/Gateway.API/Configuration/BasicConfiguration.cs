using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yarp.ReverseProxy.Configuration;
using static System.Net.WebRequestMethods;
using System.Security.Cryptography.Xml;

namespace Gateway.API.Configuration
{
    public static class BasicConfiguration
    {
        public static IReadOnlyList<RouteConfig> GetRoutes()
        {
            return new[]
            {
                new RouteConfig
                {
                    RouteId = "authRoute",
                    ClusterId = "authCluster",
                    Match = new RouteMatch
                    {
                        Path = "/api/auth/{**catch-all}"
                    }
                },
                new RouteConfig
                {
                    RouteId = "userRoute",
                    ClusterId = "authCluster",
                    Match = new RouteMatch
                    {
                        Path = "/api/users/{**catch-all}"
                    }
                },
                new RouteConfig
                {
                    RouteId = "orderRoute",
                    ClusterId = "saleCluster",
                    Match = new RouteMatch
                    {
                        Path = "/api/order/{**catch-all}"
                    }
                },
                new RouteConfig
                {
                    RouteId = "saleRoute",
                    ClusterId = "saleCluster",
                    Match = new RouteMatch
                    {
                        Path = "/api/sales/{**catch-all}"
                    }
                },
                new RouteConfig
                {
                    RouteId = "stockRoute",
                    ClusterId = "stockCluster",
                    Match = new RouteMatch
                    {
                        Path = "/api/stock/{**catch-all}"
                    }
                },
                new RouteConfig
                {
                    RouteId = "productRoute",
                    ClusterId = "stockCluster",
                    Match = new RouteMatch
                    {
                        Path = "/api/product/{**catch-all}"
                    }
                },
            };
        }
        public static IReadOnlyList<ClusterConfig> GetClusters()
        {
            return new[]
            {
                new ClusterConfig
                {
                    ClusterId = "authCluster",
                    Destinations = new Dictionary<string, DestinationConfig>
                    {
                        {"authservice", new DestinationConfig
                            {
                                Address = "http://authservice:5001/"
                            }
                        }
                    }
                },
                new ClusterConfig
                {
                    ClusterId = "saleCluster",
                    Destinations = new Dictionary<string, DestinationConfig>
                    {
                        {"saleservice", new DestinationConfig
                            {
                                Address = "http://saleservice:5002/"
                            }
                        }
                    }
                },
                new ClusterConfig
                {
                    ClusterId = "stockCluster",
                    Destinations = new Dictionary<string, DestinationConfig>
                    {
                        {"stockservice", new DestinationConfig
                            {
                                Address = "http://stockservice:5003/"
                            }
                        }
                    }
                }
            };
        }
    }
}  