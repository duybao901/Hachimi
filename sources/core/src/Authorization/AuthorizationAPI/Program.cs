using AuthorizationApi.Attributes;
using AuthorizationApi.DependencyInjection.Extensions;
using AuthorizationApi.Middleware;
using AuthorizationAPI;
using AuthorizationAPI.SeedData;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Log
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Logging.ClearProviders().AddSerilog();
builder.Host.UseSerilog();

// Add Swagger
builder.Services
        .AddSwaggerGenNewtonsoftSupport()
        .AddFluentValidationRulesToSwagger()
        .AddEndpointsApiExplorer()
        .AddSwaggerAuthorizationApi();

builder.Services
    .AddApiVersioning(options => options.ReportApiVersions = true)
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

// Api
builder.Services.AddControllers().AddApplicationPart(AuthorizationApi.AssemblyReference.Assembly);

// Service DI
builder.Services.AddSqlServerAuthorizationApi();
builder.Services.AddMediatRAuthorizationApi();
builder.Services.AddRedisAuthorizationApi(builder.Configuration);
builder.Services.AddServicesAuthorizationApi();
builder.Services.AddRepositoryBaseConfigurationPersistence();
builder.Services.AddInterceptorsConfigurationPersistence();
builder.Services.AddQuartzInfrastructure();
builder.Services.AddMasstransitRabbitMQInfrastructure(builder.Configuration);

builder.Services.AddJwtAuthenticationAPI(builder.Configuration);
builder.Services.AddScoped<CustomJwtBearerEvents>();

// Middleware
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

// **
var app = builder.Build();

// Use CORS
//app.UseRouting();
//app.UseCors("AllowFrontend");

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerAuthorizationApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

try
{
    using (var scope = app.Services.CreateScope())
    {
        await SeedRoleAsync.SeedRolesAsync(scope.ServiceProvider);
    }

    await app.RunAsync();
    Log.Information("Stopped cleanly");
}
catch (Exception ex)
{
    Log.Fatal(ex, "An unhandled exception occured during bootstrapping");
    await app.StopAsync();
}
finally
{
    Log.CloseAndFlush();
    await app.DisposeAsync();
}

