using Command.API.DependencyInjection.Extensions;
using Command.API.Middleware;
using Command.Application.DependencyInjection.Extensions;
using Command.Infrastructure.DependencyInjection.Extensions;
using Command.Persistence.DependencyInjection.Extensions;
using Command.Persistence.DependencyInjection.Options;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Log
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Logging.ClearProviders().AddSerilog();
builder.Host.UseSerilog();

// Inject Persistence Services
builder.Services.ConfigureSqlServerRetryOptionsPersistence(builder.Configuration.GetSection(nameof(SqlServerRetryOptions)));
builder.Services.AddSqlServerPersistence();
builder.Services.AddRepositoryBaseConfigurationPersistence();
builder.Services.AddInterceptorsConfigurationPersistence();

// Inject Application Services
builder.Services.AddConfigurationMediatRApplication();
builder.Services.AddConfigurationAutoMapperApplication();

// Inject Infrastructure Services
builder.Services.AddMasstransitRabbitMQInfrastructure(builder.Configuration);
builder.Services.AddQuartzInfrastructure();
builder.Services.AddServiceInfrastructure();

// Middleware 
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

// Swagger + Versioning
builder.Services
        .AddSwaggerGenNewtonsoftSupport()
        .AddFluentValidationRulesToSwagger()
        .AddEndpointsApiExplorer()
        .AddSwaggerApi();

builder.Services
    .AddApiVersioning(options => options.ReportApiVersions = true)
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

// Api
builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new RouteTokenTransformerConvention(new LowercaseControllerTransformer()));
})
    .AddApplicationPart(Command.Presentation.AssemblyReference.Assembly);

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.ConfigureSwaggerApi();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

//app.UseAuthorization();

app.MapControllers();

app.Run();
