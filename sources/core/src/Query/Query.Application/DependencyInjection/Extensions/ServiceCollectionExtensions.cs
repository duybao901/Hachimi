using Microsoft.Extensions.DependencyInjection;
using Query.Application.Mapper;

namespace Query.Application.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMediatRApplication(this IServiceCollection services)
     => services.AddMediatR(config =>
         config.RegisterServicesFromAssembly(AssemblyReference.Assembly));

    public static IServiceCollection AddConfigurationAutoMapper(this IServiceCollection services)
    {
        return services.AddAutoMapper(typeof(ServiceProfile));
    }
}
