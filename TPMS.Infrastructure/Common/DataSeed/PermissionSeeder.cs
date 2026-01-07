using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Infrastructure.Common.DataSeed;

using Microsoft.EntityFrameworkCore;

public static class PermissionSeeder
{
    public static async Task SeedAsync(TPMSDBContext context)
    {
        var existingPermissions = await context.Permissions
            .Select(p => p.PermissionName)
            .ToListAsync();

        var newPermissions = PermissionSeedData.GetPermissions()
            .Where(p => !existingPermissions.Contains(p.PermissionName))
            .ToList();

        if (newPermissions.Any())
        {
            context.Permissions.AddRange(newPermissions);
            await context.SaveChangesAsync();
        }
    }
}
