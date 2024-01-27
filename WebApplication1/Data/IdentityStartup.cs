using Microsoft.AspNetCore.Identity;
using WebApplication1.Models;

namespace WebApplication1.Data;

public class IdentityStartup
{
    public static async Task CreateRoles(RoleManager<IdentityRole> roleManager)
    {
        await AddRoleIfNotExists(roleManager, RoleNames.Administrator);
        await AddRoleIfNotExists(roleManager, RoleNames.Customer);
    }

    public static async Task CreateUsers(UserManager<ApplicationUser> userManager)
    {
        await AddUserIfNotExists(userManager, ("admin@admin", "123456", RoleNames.Administrator));
        await AddUserIfNotExists(userManager, ("kazkas@kazkas", "123456", RoleNames.Customer));
    }

    private static async Task AddUserIfNotExists(UserManager<ApplicationUser> userManager, (string name, string password, string role) demoUser)
    {
        var user = await userManager.FindByNameAsync(demoUser.name);
        if (user is not null) return;

        var newUser = new ApplicationUser
        {
            UserName = demoUser.name,
            Email = demoUser.name,
            EmailConfirmed = true
        };
        _ = await userManager.CreateAsync(newUser, demoUser.password);

        if (!string.IsNullOrWhiteSpace(demoUser.role))
        {
            await userManager.AddToRoleAsync(newUser, demoUser.role);
        }
    }

    private static async Task AddRoleIfNotExists(RoleManager<IdentityRole> roleManager, string roleName)
    {
        var role = await roleManager.FindByNameAsync(roleName);
        if (role is not null) return;

        await roleManager.CreateAsync(new IdentityRole { Name = roleName });
    }
}