using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Query.Domain.Abstractions.Options;

namespace Query.Infrastructure.DependencyInjection.Extensions;
public static class ServiceCollectionExtensions
{

    public static void ConfigureMongoDbSettingInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Bind cấu hình từ appsettings.json vào MongoDbSettings
        services.Configure<MongoDbSettings>(configuration.GetSection(nameof(MongoDbSettings)));

        // 1. Đăng ký sử dụng IOptions<MongoDbSettings>
        services.AddSingleton<IMongoDbSettings>((serviceProvider) =>
            serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value);
    }
}
