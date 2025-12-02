using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AuthorizationAPI.DependencyInjection.Options;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, new OpenApiInfo
            {
                Title = AppDomain.CurrentDomain.FriendlyName,
                Version = description.ApiVersion.ToString(),
                Description = $"API {description.ApiVersion}",
                Contact = new OpenApiContact
                {
                    Name = "Hachimi",
                    Email = "hachimi@gmail.com",
                    Url = new Uri("https://hachimi.com")
                },
                License = new OpenApiLicense
                {
                    Name = "MIT License",
                    Url = new Uri("https://opensource.org/licenses/MIT")
                }

            });
        }

        options.MapType<DateOnly>(() => new OpenApiSchema
        {
            Type = "string",
            Format = "date",
            Example = new OpenApiString(DateOnly.MinValue.ToString("yyyy-MM-dd"))
        });

        options.CustomSchemaIds(type => type.ToString().Replace("+", "."));


        //var xmlFiles = $"{Presentation.AssemblyReference.Assembly.GetName().Name}.xml"; // -> Command.Presentation.xml
        //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFiles);

        //options.IncludeXmlComments(xmlPath);
    }
}
