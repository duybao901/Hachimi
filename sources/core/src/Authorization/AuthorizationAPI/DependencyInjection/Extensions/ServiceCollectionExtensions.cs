using AuthorizationApi.Abstractions;
using AuthorizationApi.Behaviors;
using AuthorizationApi.Caching;
using AuthorizationAPI;
using AuthorizationAPI.Abstractions;
using AuthorizationAPI.Abstractions.Repositories;
using AuthorizationAPI.Authentication;
using AuthorizationAPI.BackgroundJobs;
using AuthorizationAPI.Behaviors;
using AuthorizationAPI.DependencyInjection.Extensions;
using AuthorizationAPI.DependencyInjection.Options;
using AuthorizationAPI.Entities.Identity;
using AuthorizationAPI.Interceptors;
using AuthorizationAPI.PipelineObservers;
using AuthorizationAPI.Repositories;
using Contract.JsonConverters;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz;
using Quartz.Simpl;
using System.Reflection;

namespace AuthorizationApi.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddSqlServerAuthorizationApi(this IServiceCollection services)
    {
        services.AddDbContextPool<DbContext, ApplicationDbContext>((IServiceProvider provider, DbContextOptionsBuilder builder) =>
        {
            var converDomainEventsToOutboxInterceptor = provider.GetRequiredService<ConvertDomainEventsToOutboxMessageInterceptor>();
            var UpdateAuditEntitiesInterceptor = provider.GetRequiredService<UpdateAuditableEntitiesInterceptor>();

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
            .AddInterceptors(
                converDomainEventsToOutboxInterceptor,
                UpdateAuditEntitiesInterceptor);
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

        services.AddIdentityCore<AppUser>(options =>
        {
            options.User.RequireUniqueEmail = false;
            options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";

            // Password settings (using in FluentValidation)
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 0;

            // Lockout settings
            options.Lockout.AllowedForNewUsers = true;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
            options.Lockout.MaxFailedAccessAttempts = 3;
        })
        .AddRoles<AppRole>()                              // RoleManager support
        .AddSignInManager<SignInManager<AppUser>>()       // Login support (optional)
        .AddEntityFrameworkStores<ApplicationDbContext>() // EF Core persistence
        .AddDefaultTokenProviders();                      // Reset password, email confirm, security stamp

    }

    public static void AddMediatRAuthorizationApi(this IServiceCollection services)
        => services.AddMediatR(config =>
             config.RegisterServicesFromAssembly(AssemblyReference.Assembly))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionPipelineBehavior<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>))
            .AddValidatorsFromAssembly(Contract.AssemblyReference.Assembly, includeInternalTypes: true);
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
        services.AddTransient<IJwtTokenService, JwtTokenService>();
        services.AddTransient<ICacheService, CacheService>();
    }

    public static void AddRepositoryBaseConfigurationPersistence(this IServiceCollection services)
    {
        services.AddTransient(typeof(IRepositoryBase<,>), typeof(RepositoryBase<,>));
        services.AddTransient<IUnitOfWork, EFUnitOfWork>();
        // Do NOT use TransactionScope when using SQL retry strategies
        // EF Core cannot retry operations safely inside TransactionScope.
    }

    public static void AddInterceptorsConfigurationPersistence(this IServiceCollection services)
    {
        services.AddSingleton<ConvertDomainEventsToOutboxMessageInterceptor>();
        services.AddSingleton<UpdateAuditableEntitiesInterceptor>();
    }

    // Configure MassTransit with RabbitMQ
    public static IServiceCollection AddMasstransitRabbitMQInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var masstransitConfiguration = new MasstransitConfigurationOption();
        configuration.GetSection(nameof(MasstransitConfigurationOption)).Bind(masstransitConfiguration);

        var messageBusOption = new MessageBusOption();
        configuration.GetSection(nameof(MessageBusOption)).Bind(messageBusOption);

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


    // Configure Job
    public static void AddQuartzInfrastructure(this IServiceCollection services)
    {
        services.AddQuartz(config =>
        {
            var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

            config.AddJob<ProcessOutboxMessagesJob>(jobKey, (serviceProvider, jobConfigurator) =>
            {
            })
            .AddTrigger(
                trigger =>
                    trigger.ForJob(jobKey)
                        .WithSimpleSchedule(
                            schedule =>
                                schedule.WithInterval(TimeSpan.FromMilliseconds(100))
                                    .RepeatForever()));
            // UseMicrosoftDependencyInjectionJobFactory() → Quartz call DI container to create instance job (ILogger,IEmail,ApplicationDbContext...)
            config.UseJobFactory<MicrosoftDependencyInjectionJobFactory>();
        });

        services.AddQuartzHostedService();
    }
}
