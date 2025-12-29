using Contract.JsonConverters;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ProjectionWorker.Abstractions;
using ProjectionWorker.Abstractions.Options;
using ProjectionWorker.Abstractions.Repositories;
using ProjectionWorker.DependencyInjection.Options;
using ProjectionWorker.PipelineObservers;
using ProjectionWorker.Repositories;
using System.Reflection;

namespace ProjectionWorker.DependencyInjection.Extentions;
public static class ServiceCollectionExtensions
{
    public static void AddSqlServerPersistence(this IServiceCollection services)
    {
        services.AddDbContextPool<DbContext, ReadDbContext>((provider, builder) =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            var options = provider.GetRequiredService<IOptionsMonitor<SqlServerRetryOptions>>();

            #region ================= SQL-SERVER-STRATEGY-1 =================

            builder
            .EnableDetailedErrors(true)
            .EnableSensitiveDataLogging(true)
            //.UseLazyLoadingProxies(true) // => If UseLazyLoadingProxies, all of the navigation fields should be VIRTUAL
            .UseSqlServer(
                connectionString: configuration.GetConnectionString("connectionStrings"),
                sqlServerOptionsAction: optionsBuilder
                        => optionsBuilder.ExecutionStrategy(
                                dependencies => new SqlServerRetryingExecutionStrategy(
                                    dependencies: dependencies,
                                    maxRetryCount: options.CurrentValue.MaxRetryCount,
                                    maxRetryDelay: options.CurrentValue.MaxRetryDelay,
                                    errorNumbersToAdd: options.CurrentValue.ErrorNumbersToAdd))
                            .MigrationsAssembly(typeof(ReadDbContext).Assembly.GetName().Name));
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


    public static void ConfigureMongoDbSettingInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Bind cấu hình từ appsettings.json vào MongoDbSettings
        services.Configure<MongoDbSettings>(configuration.GetSection(nameof(MongoDbSettings)));

        // 1. Đăng ký sử dụng IOptions<MongoDbSettings>
        services.AddSingleton<IMongoDbSettings>((serviceProvider) =>
            serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value);
    }

    public static IServiceCollection AddMasstransitRabbitMQInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var masstransitConfiguration = new MasstransitConfigurationOptions();
        configuration.GetSection(nameof(MasstransitConfigurationOptions)).Bind(masstransitConfiguration);

        var messageBusOption = new MessageBusOptions();
        configuration.GetSection(nameof(MessageBusOptions)).Bind(messageBusOption);

        services.AddMassTransit(cfg =>
        {
            // ===================== Setup for Consumer =====================
            cfg.AddConsumers(Assembly.GetExecutingAssembly()); // Add all of consumers to masstransit instead above command

            // ?? => Configure endpoint formatter. Not configure for producer Root Exchange
            cfg.SetKebabCaseEndpointNameFormatter(); // ??

            cfg.UsingRabbitMq((context, bus) =>
            {
                bus.Host(masstransitConfiguration.Host, masstransitConfiguration.Port, masstransitConfiguration.VHost, h =>
                {
                    h.Username(masstransitConfiguration.UserName);
                    h.Password(masstransitConfiguration.Password);
                });

                bus.UseMessageRetry(retry
                => retry.Incremental(
                           retryLimit: messageBusOption.RetryLimit,
                           initialInterval: messageBusOption.InitialInterval,
                           intervalIncrement: messageBusOption.IntervalIncrement));

                bus.UseNewtonsoftJsonSerializer();

                bus.ConfigureNewtonsoftJsonSerializer(settings =>
                {
                    settings.Converters.Add(new TypeNameHandlingConverter(TypeNameHandling.Objects));
                    settings.Converters.Add(new DateOnlyJsonConverter());
                    settings.Converters.Add(new ExpirationDateOnlyJsonConverter());
                    return settings;
                });

                bus.ConfigureNewtonsoftJsonDeserializer(settings =>
                {
                    settings.Converters.Add(new TypeNameHandlingConverter(TypeNameHandling.Objects));
                    settings.Converters.Add(new DateOnlyJsonConverter());
                    settings.Converters.Add(new ExpirationDateOnlyJsonConverter());
                    return settings;
                });

                bus.ConnectReceiveObserver(new LoggingReceiveObserver());
                bus.ConnectConsumeObserver(new LoggingConsumeObserver());
                bus.ConnectPublishObserver(new LoggingPublishObserver());
                bus.ConnectSendObserver(new LoggingSendObserver());

                // Rename for Root Exchange and setup for consumer also
                bus.MessageTopology.SetEntityNameFormatter(new KebabCaseEntityNameFormatter());

                // ===================== Setup for Consumer =====================

                // Importantce to create Echange and Queue
                bus.ConfigureEndpoints(context);
            });
        });

        return services;
    }

    public static void AddRepositoryBaseConfigurationPersistence(this IServiceCollection services)
    {
        services.AddTransient(typeof(IRepositoryBase<,>), typeof(RepositoryBase<,>));
        services.AddTransient<IUnitOfWork, EFUnitOfWork>();
        services.AddTransient(typeof(IMongoRepository<>), typeof(MongoRepository<>));
    }

    // MediatR
    public static void AddMediatRInfrastructure(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(AssemblyReference.Assembly);
        });
    }
}
