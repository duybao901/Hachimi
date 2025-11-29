using Command.API.DependencyInjection.Options;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;


namespace Command.API.DependencyInjection.Extensions;

public static class SwaggerExtensions
{
    public static void AddSwaggerApi(this IServiceCollection services)
    {
        services.AddSwaggerGen();
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
    }

    public static void ConfigureSwaggerApi(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            foreach (var version in app.DescribeApiVersions().Select(version => version.GroupName))
                options.SwaggerEndpoint($"/swagger/{version}/swagger.json", version);

            // Override UI default
            options.IndexStream = () => File.OpenRead("wwwroot/swagger/ui/index.html");

            // Inject custom CSS
            options.InjectStylesheet("/swagger/ui/custom.css");

            options.DisplayRequestDuration();
            options.EnableTryItOutByDefault();
            options.DocExpansion(DocExpansion.Full);
        });

        //app.MapGet("/", () => Results.Redirect("/swagger/index.html"))
        //    .WithTags(string.Empty);
    }
}
