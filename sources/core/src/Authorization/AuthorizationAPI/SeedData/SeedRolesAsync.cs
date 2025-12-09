using AuthorizationAPI.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace AuthorizationAPI.SeedData;
public static class SeedRoleAsync
{
    public static async Task SeedRolesAsync(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<AppRole>>();

        if (!await roleManager.RoleExistsAsync("Author"))
        {
            await roleManager.CreateAsync(new AppRole
            {
                Name = "Author",
                NormalizedName = "AUTHOR",
                Description = "Default author role",
                RoleCode = "AUTHOR"
            });
        }
    }
}
