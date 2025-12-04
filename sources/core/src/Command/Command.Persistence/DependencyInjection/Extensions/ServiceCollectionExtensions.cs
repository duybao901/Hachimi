using Command.Domain.Abstractions;
using Command.Domain.Abstractions.Repositories;
using Command.Persistence.DependencyInjection.Options;
using Command.Persistence.Interceptors;
using Command.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Command.Persistence.DependencyInjection.Extensions;
public static class ServiceCollectionExtensions
{
    public static void AddSqlServerPersistence(this IServiceCollection services)
    {
        services.AddDbContextPool<DbContext, ApplicationDbContext>((IServiceProvider provider, DbContextOptionsBuilder builder) =>
        {
            var convertDomainEventsToOutboxMessageInterceptor = provider.GetRequiredService<ConvertDomainEventsToOutboxMessageInterceptor>();
            var updateAuditableEntitiesInterceptor = provider.GetRequiredService<UpdateAuditableEntitiesInterceptor>();

            var configuration = provider.GetRequiredService<IConfiguration>();
            var options = provider.GetRequiredService<IOptionsMonitor<SqlServerRetryOptions>>();

            #region ================= SQL-SERVER-STRATEGY-1 =================

            builder
            .EnableDetailedErrors(true)
            .EnableSensitiveDataLogging(true)
            .UseLazyLoadingProxies(true) // => If UseLazyLoadingProxies, all of the navigation fields should be VIRTUAL
            .UseSqlServer(
                connectionString: configuration.GetConnectionString("connectionStrings"),
                sqlServerOptionsAction: optionsBuilder
                        => optionsBuilder.ExecutionStrategy(
                                dependencies => new SqlServerRetryingExecutionStrategy(
                                    dependencies: dependencies,
                                    maxRetryCount: options.CurrentValue.MaxRetryCount,
                                    maxRetryDelay: options.CurrentValue.MaxRetryDelay,
                                    errorNumbersToAdd: options.CurrentValue.ErrorNumbersToAdd))
                            .MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name))
            .AddInterceptors(convertDomainEventsToOutboxMessageInterceptor, updateAuditableEntitiesInterceptor);
            #endregion ================= SQL-SERVER-STRATEGY-1 =================

            #region ============== SQL-SERVER-STRATEGY-2 ==============

            //builder
            //.EnableDetailedErrors(true)
            //.EnableSensitiveDataLogging(true)
            //.UseLazyLoadingProxies(true) // => If UseLazyLoadingProxies, all of the navigation fields should be VIRTUAL
            //.UseSqlServer(
            //    connectionString: configuration.GetConnectionString("ConnectionStrings"),
            //        sqlServerOptionsAction: optionsBuilder
            //            => optionsBuilder
            //            .MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name));

            #endregion ============== SQL-SERVER-STRATEGY-2 ==============
        });
    }

    public static OptionsBuilder<SqlServerRetryOptions> ConfigureSqlServerRetryOptionsPersistence(this IServiceCollection services, IConfigurationSection section)
         => services
             .AddOptions<SqlServerRetryOptions>()
             .Bind(section)
             .ValidateDataAnnotations()
             .ValidateOnStart(); // validate options as soon as the application starts


    public static void AddRepositoryBaseConfigurationPersistence(this IServiceCollection services)
    {
        services.AddTransient(typeof(IRepositoryBase<,>), typeof(RepositoryBase<,>));
        services.AddTransient<IUnitOfWork, EFUnitOfWork>();
    }

    public static void AddInterceptorsConfigurationPersistence(this IServiceCollection services)
    {
        services.AddSingleton<ConvertDomainEventsToOutboxMessageInterceptor>();
        services.AddSingleton<UpdateAuditableEntitiesInterceptor>();
    }
}
