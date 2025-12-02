using AuthorizationApi.Attributes;
using AuthorizationApi.DependencyInjection.Extensions;
using AuthorizationApi.Middleware;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

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
builder.Services.AddMediatRAuthorizationApi();
builder.Services.AddRedisAuthorizationApi(builder.Configuration);
builder.Services.AddServicesAuthorizationApi();

builder.Services.AddJwtAuthenticationAPI(builder.Configuration);
builder.Services.AddScoped<CustomJwtBearerEvents>();

// Middleware
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

var app = builder.Build();

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

