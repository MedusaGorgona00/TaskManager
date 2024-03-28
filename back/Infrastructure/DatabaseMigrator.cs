using Domain.Entities;
using Domain.Entities.Users;
using Domain.Common.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Infrastructure
{
    public class DatabaseMigrator
    {
        public static async Task SeedDatabaseAsync(IServiceProvider appServiceProvider)
        {
            await using var scope = appServiceProvider.CreateAsyncScope();
            var serviceProvider = scope.ServiceProvider;
            var logger = serviceProvider.GetRequiredService<ILogger<DatabaseMigrator>>();
            try
            {
                var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
                await context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Migration error");
            }

            try
            {
                var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
                var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
                await SeedRolesAsync(roleManager);
                await SeedAdminAsync(userManager);

                //var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
                // await SeedMockDataAsync(context);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "DB seed error");
            }
        }

        private static async Task SeedAdminAsync(UserManager<User> userManager)
        {
            if (await userManager.Users.AllAsync(us => us.Email != "admin@taskmanager.com"))
            {
                var admin = new User
                {
                    Email = "admin@taskmanager.com",
                    UserName = "admin@taskmanager.com",
                    EmailConfirmed = true,
                    PhoneNumber = "+10000000000",
                    PhoneNumberConfirmed = true,
                    FirstName = "Admin",
                    LastName = "Admin",
                    LockoutEnabled = false,
                };

                await userManager.CreateAsync(admin, "12qw!@QW");
                await userManager.AddToRoleAsync(admin, nameof(UserRoleEnum.Admin));
            }

        }

        private static async Task SeedRolesAsync(RoleManager<Role> roleManager)
        {
            foreach (UserRoleEnum role in Enum.GetValues(typeof(UserRoleEnum)))
            {
                var normalizedRole = role.ToString();
                var dbRole = roleManager.Roles.FirstOrDefault(r => r.NormalizedName == normalizedRole);
                if (dbRole == null)
                {
                    var result = await roleManager.CreateAsync(new Role { Name = role.ToString() });
                    dbRole = roleManager.Roles.FirstOrDefault(r => r.NormalizedName == normalizedRole);
                }

                if (role == UserRoleEnum.Admin && dbRole != null)
                {
                    await roleManager.UpdateAsync(dbRole);
                }
            }
        }
    
    }

}
