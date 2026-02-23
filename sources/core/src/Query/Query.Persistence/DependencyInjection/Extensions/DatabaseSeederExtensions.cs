using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Query.Persistence.DependencyInjection.Extensions;

public static class DatabaseSeederExtensions
{
    public static async Task SeedDatabaseAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>()
            .CreateLogger("DatabaseSeeder");

        var seeders = scope.ServiceProvider.GetServices<IDataSeeder>();

        foreach (var seeder in seeders)
        {
            try
            {
                await seeder.SeedAsync();
                logger.LogInformation("Seeder {SeederName} executed successfully.",
                    seeder.GetType().Name);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                    "Seeder {SeederName} failed.",
                    seeder.GetType().Name);
                throw; 
            }
        }
    }
}
