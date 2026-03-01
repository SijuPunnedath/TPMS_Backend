using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TPMS.Domain.Entities;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasData(
            new Role
            {
                RoleID = 1,
                RoleName = "Admin",
                Description = "System administrator with full access",
                IsActive = true,
                CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 0, 0, 0), DateTimeKind.Utc)
            },
            new Role
            {
                RoleID = 2,
                RoleName = "Property Owner",
                Description = "Owner of properties",
                IsActive = true,
                CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 0, 0, 0), DateTimeKind.Utc)
            },
            new Role
            {
                RoleID = 3,
                RoleName = "Property Manager",
                Description = "Manages properties for owners",
                IsActive = true,
                CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 0, 0, 0), DateTimeKind.Utc)
                

            },
            new Role
            {
                RoleID = 4,
                RoleName = "Tenant",
                Description = "Tenant or lessee",
                IsActive = true,
                CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 0, 0, 0), DateTimeKind.Utc),

            },
            new Role
            {
                RoleID = 5,
                RoleName = "Vendor",
                Description = "Service or maintenance vendor",
                IsActive = true,
                CreatedAt = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 0, 0, 0), DateTimeKind.Utc),
                

            }
        );
    }
}