using Microsoft.AspNetCore.Identity;

namespace Cinema.Models
{
    public static class SeedDatabase
    {
        public static async Task SeedRolesAsync(RoleManager<AppRole> roleManager)
        {
            string[] roles = new[] { "Admin", "User" };

            foreach (var roleName in roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new AppRole { Name = roleName });
                }
            }
        }

        public static async Task SeedAdminUserAsync(UserManager<AppUser> userManager)
        {
            var adminEmail = "admin@cinema.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new AppUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "User"
                };

                await userManager.CreateAsync(adminUser, "Admin123");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
