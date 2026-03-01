using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TPMS.Domain.Entities;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Infrastructure.Persistence
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(TPMSDBContext context)
        {
            // Ensure DB created
           // await context.Database.MigrateAsync();

            // Check if admin already exists
            if (await context.Users.AnyAsync())
                return;

            // Make sure Role with ID = 1 exists
            var adminRole = await context.Roles
                .FirstOrDefaultAsync(r => r.RoleID == 1);

            if (adminRole == null)
            {
                adminRole = new Role
                {
                    RoleID = 1,
                    RoleName = "Admin",
                    CreatedAt = DateTime.UtcNow
                };

                context.Roles.Add(adminRole);
                await context.SaveChangesAsync();
            }

            var adminUser = new User
            {
                Username = "Admin",
                Email = "admin@tpms.local",
                RoleID = adminRole.RoleID,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var hasher = new PasswordHasher<User>();
            adminUser.PasswordHash = hasher.HashPassword(adminUser, "Admin@123");

            context.Users.Add(adminUser);
            await context.SaveChangesAsync();
        }
    }
}