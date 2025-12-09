using Microsoft.AspNetCore.Identity;
using TalentoPlus.Web.Data;

namespace TalentoPlus.Web.Data;

public static class DbSeeder
{
    public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

        string[] roleNames = { "Admin", "User" };
        IdentityResult roleResult;

        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // Create Admin User
        var adminEmail = "admin@talentoplus.com";
        var adminPassword = "Admin123."; // Password to enforce

        var adminUser = new IdentityUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };

        var user = await userManager.FindByEmailAsync(adminEmail);

        if (user == null)
        {
            var createPowerUser = await userManager.CreateAsync(adminUser, adminPassword);
            if (createPowerUser.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
                Console.WriteLine($"[DbSeeder] Admin user '{adminEmail}' created successfully.");
            }
            else
            {
                Console.WriteLine($"[DbSeeder] Error creating admin user: {string.Join(", ", createPowerUser.Errors.Select(e => e.Description))}");
            }
        }
        else
        {
            Console.WriteLine($"[DbSeeder] Admin user '{adminEmail}' already exists.");

            // Check if password needs update (simplified check by trying to sign in or just resetting token)
            // For development simplicity, we will force reset the password to ensure it matches the code
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var result = await userManager.ResetPasswordAsync(user, token, adminPassword);

            if (result.Succeeded)
            {
                Console.WriteLine($"[DbSeeder] Admin user '{adminEmail}' password updated/verified.");
            }
            else
            {
                Console.WriteLine($"[DbSeeder] Error updating admin password: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            // Ensure role is assigned
            if (!await userManager.IsInRoleAsync(user, "Admin"))
            {
                await userManager.AddToRoleAsync(user, "Admin");
                Console.WriteLine($"[DbSeeder] Role 'Admin' assigned to user '{adminEmail}'.");
            }
        }
    }
}
