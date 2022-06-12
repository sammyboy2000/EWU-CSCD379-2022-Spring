using Microsoft.AspNetCore.Identity;
using Wordle.Api.Data;

namespace Wordle.Api.Identity
{
    public static class IdentitySeed
    {
        public static async Task SeedAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Seed Roles
            await SeedRolesAsync(roleManager);

            // Seed Admin User
            await SeedAdminUserAsync(userManager);
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            // Seed Roles
            if (!await roleManager.RoleExistsAsync(Roles.Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
            }
            if (!await roleManager.RoleExistsAsync(Roles.Grant))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Grant));
            }
            if (!await roleManager.RoleExistsAsync(Roles.MasterOfTheUniverse))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.MasterOfTheUniverse));
            }
        }

        private static async Task SeedAdminUserAsync(UserManager<AppUser> userManager)
        {
            // Seed Admin/Grant User
            if (await userManager.FindByNameAsync("Admin@intellitect.com") == null)
            {
                AppUser user = new AppUser
                {
                    UserName = "Admin@intellitect.com",
                    Email = "Admin@intellitect.com",
                    Birthday = new DateTime(2000, 12, 24),
                };

                IdentityResult result = userManager.CreateAsync(user, "P@ssw0rd123").Result;

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, Roles.Admin);
                    await userManager.AddToRoleAsync(user, Roles.Grant);
                }

            }

            // Seed Administrator not Grant User
            if (await userManager.FindByNameAsync("Administrator@intellitect.com") == null)
            {
                AppUser user = new AppUser
                {
                    UserName = "Administrator@intellitect.com",
                    Email = "Administrator@intellitect.com",
                    Birthday = new DateTime(1111, 11, 11),
                };

                IdentityResult result = userManager.CreateAsync(user, "P@ssw0rd123").Result;

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, Roles.Admin);
                }

            }

            // Seed Orko User
            if (await userManager.FindByNameAsync("Orko@intellitect.com") == null)
            {
                AppUser user = new AppUser
                {
                    UserName = "Orko@intellitect.com",
                    Email = "Orko@intellitect.com",
                    Birthday = new DateTime(2020, 1, 21),
                };

                IdentityResult result = userManager.CreateAsync(user, "P@ssw0rd123").Result;

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, Roles.Admin);
                    await userManager.AddToRoleAsync(user, Roles.MasterOfTheUniverse);
                }

            }

            // Seed HeMan User
            if (await userManager.FindByNameAsync("HeMan@intellitect.com") == null)
            {
                AppUser user = new AppUser
                {
                    UserName = "HeMan@intellitect.com",
                    Email = "HeMan@intellitect.com",
                    Birthday = new DateTime(1983, 11, 5),
                };

                IdentityResult result = userManager.CreateAsync(user, "P@ssw0rd123").Result;

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, Roles.Admin);
                    await userManager.AddToRoleAsync(user, Roles.MasterOfTheUniverse);
                }

            }

            // Seed Guest with Grant Role User
            if (await userManager.FindByNameAsync("Guest@intellitect.com") == null)
            {
                AppUser user = new AppUser
                {
                    UserName = "Guest@intellitect.com",
                    Email = "Guest@intellitect.com",
                    Birthday = DateTime.Today,
                };

                IdentityResult result = userManager.CreateAsync(user, "P@ssw0rd123").Result;
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, Roles.Grant);
                }
            }
        }
    }
}
