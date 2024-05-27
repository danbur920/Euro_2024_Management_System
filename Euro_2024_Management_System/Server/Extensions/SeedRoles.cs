using Euro_2024_Management_System.Server.Models;
using Microsoft.AspNetCore.Identity;

namespace Euro_2024_Management_System.Server.Extensions
{
    public static class SeedRoles
    {
        public static async Task Initialize(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            string roleName = "Admin";

            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }
}
