using ApiGateway.Abstractions;
using ApiGateway.Caching;
using System.Security.Claims;
using Yarp.ReverseProxy.Transforms;

namespace ApiGateway.DepencencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddReverseProxyApiGateway(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddReverseProxy().LoadFromConfig(configuration.GetSection("ReverseProxy")).AddTransforms(builderContext =>
        {
            builderContext.AddRequestTransform(async context =>
            {
                var user = context.HttpContext.User;

                if (user?.Identity?.IsAuthenticated == true)
                {
                    context.ProxyRequest.Headers.Remove("X-User-Id");

                    context.ProxyRequest.Headers.Add(
                        "X-User-Id",
                        user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    );
                }
            });
        }); 
    }

    public static void AddRedisAuthorizationApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(redisOptions =>
        {
            var redisConnectionString = configuration.GetConnectionString("Redis");
            redisOptions.Configuration = redisConnectionString;
        });
    }

    public static void AddServicesAuthorizationApi(this IServiceCollection services)
    {
        services.AddTransient<ICacheService, CacheService>();
    }
}
