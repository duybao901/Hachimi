using ApiGateway.Abstractions;
using ApiGateway.Caching;

namespace ApiGateway.DepencencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddReverseProxyApiGateway(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddReverseProxy().LoadFromConfig(configuration.GetSection("ReverseProxy"));
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
