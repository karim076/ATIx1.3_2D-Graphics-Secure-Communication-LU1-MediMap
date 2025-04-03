using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Models;
using System.Threading.Tasks;

namespace MediMapAPI.Service
{
    public static class RoleSeeder
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

            string[] roleNames = { "User", "Admin" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    // Create the role using RoleManager
                    var result = await roleManager.CreateAsync(new ApplicationRole { Name = roleName });
                    if (result.Succeeded)
                    {
                        logger.LogInformation($"Role '{roleName}' created successfully.");
                    }
                    else
                    {
                        logger.LogError($"Failed to create role '{roleName}'. Errors: {string.Join(", ", result.Errors)}");
                    }
                }
                else
                {
                    logger.LogInformation($"Role '{roleName}' already exists.");
                }
            }
        }
        public static async Task UpdateRoleConcurrencyStampsAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

            // Fetch all roles from the database
            var roles = roleManager.Roles.ToList();

            foreach (var role in roles)
            {
                // Check if the ConcurrencyStamp is null or empty
                if (string.IsNullOrEmpty(role.ConcurrencyStamp))
                {
                    // Update the ConcurrencyStamp
                    role.ConcurrencyStamp = Guid.NewGuid().ToString();
                    var result = await roleManager.UpdateAsync(role);

                    if (result.Succeeded)
                    {
                        logger.LogInformation($"Updated ConcurrencyStamp for role '{role.Name}'.");
                    }
                    else
                    {
                        logger.LogError($"Failed to update ConcurrencyStamp for role '{role.Name}'. Errors: {string.Join(", ", result.Errors)}");
                    }
                }
            }
        }
    }
}