using ProjectionWorker.DependencyInjection.Extentions;
using ProjectionWorker.DependencyInjection.Options;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);
//builder.Services.AddHostedService<Worker>();

// Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

builder.Services.ConfigureSqlServerRetryOptionsPersistence(builder.Configuration.GetSection(nameof(SqlServerRetryOptions)));
builder.Services.AddSqlServerPersistence();
builder.Services.AddRepositoryBaseConfigurationPersistence();
builder.Services.ConfigureMongoDbSettingInfrastructure(builder.Configuration);
builder.Services.AddMasstransitRabbitMQInfrastructure(builder.Configuration);
builder.Services.AddMediatRInfrastructure();

var host = builder.Build();
host.Run();
