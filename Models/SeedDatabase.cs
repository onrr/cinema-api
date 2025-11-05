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

            var userEmail = "john@doe.com";
            var user = await userManager.FindByEmailAsync(userEmail);

            if (user == null)
            {
                user = new AppUser
                {
                    UserName = userEmail,
                    Email = userEmail,
                    FirstName = "John",
                    LastName = "Doe"
                };

                await userManager.CreateAsync(user, "12345678Aa");
                await userManager.AddToRoleAsync(user, "User");
            }
        }
    }
}
