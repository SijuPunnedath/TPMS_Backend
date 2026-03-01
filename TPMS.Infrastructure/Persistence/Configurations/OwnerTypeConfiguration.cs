using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TPMS.Domain.Entities;

namespace TPMS.Infrastructure.Persistence.Configurations
{
    public class OwnerTypeConfiguration : IEntityTypeConfiguration<OwnerType>
    {
        public void Configure(EntityTypeBuilder<OwnerType> builder)
        {
           /* builder.ToTable("OwnerTypes");

            // 🔹 Primary Key
            builder.HasKey(x => x.OwnerTypeID);

            builder.Property(x => x.OwnerTypeID)
                   .ValueGeneratedNever(); // Required for seeding fixed IDs

            // 🔹 Properties
            builder.Property(x => x.Name)
                   .HasMaxLength(100);

            builder.Property(x => x.Description)
                   .HasMaxLength(500);

            builder.Property(x => x.IsActive)
                   .IsRequired();

            builder.Property(x => x.IsDeleted)
                   .IsRequired()
                   .HasDefaultValue(false);

            // 🔹 Audit fields
            builder.Property(x => x.CreatedAt)
                   .IsRequired();

            builder.Property(x => x.UpdatedAt)
                   .IsRequired(false);

            builder.Property(x => x.CreatedBy)
                   .IsRequired(false);

            builder.Property(x => x.UpdatedBy)
                   .IsRequired(false); */

            //  Seed Data
            builder.HasData(
                new OwnerType
                {
                    OwnerTypeID = 7,
                    Name = "General",
                    Description = "General owner type. This can be useful to add general documents",
                    IsActive = true,
                    IsDeleted = false,
                    CreatedAt = DateTime.SpecifyKind(new DateTime(2026, 1, 1, 0, 0, 0), DateTimeKind.Utc),
                },
                new OwnerType
                {
                    OwnerTypeID = 2,
                    Name = "Landlord",
                    Description = "Owner type Landlord",
                    IsActive = true,
                    IsDeleted = false,
                    CreatedAt = DateTime.SpecifyKind(new DateTime(2026, 1, 1, 0, 0, 0), DateTimeKind.Utc),
                },
                new OwnerType
                {
                    OwnerTypeID = 1,
                    Name = "Property",
                    Description = "Owner type Property",
                    IsActive = true,
                    IsDeleted = false,
                    CreatedAt = DateTime.SpecifyKind(new DateTime(2026, 1, 1, 0, 0, 0), DateTimeKind.Utc),
                },
                new OwnerType
                {
                    OwnerTypeID = 3,
                    Name = "Tenant",
                    Description = "Owner type Tenant",
                    IsActive = true,
                    IsDeleted = false,
                    CreatedAt = DateTime.SpecifyKind(new DateTime(2026, 1, 1, 0, 0, 0), DateTimeKind.Utc),
                },
                new OwnerType
                {
                    OwnerTypeID = 5,
                    Name = "Vendor",
                    Description = "Vendor of the company",
                    IsActive = true,
                    IsDeleted = false,
                    CreatedAt = DateTime.SpecifyKind(new DateTime(2026, 1, 1, 0, 0, 0), DateTimeKind.Utc),
                },
                new OwnerType
                {
                    OwnerTypeID = 4,
                    Name = "Lease",
                    Description = "Lease of the company ",
                    IsActive = true,
                    IsDeleted = false,
                    CreatedAt = DateTime.SpecifyKind(new DateTime(2026, 1, 1, 0, 0, 0), DateTimeKind.Utc),
                }
            );
        }
    }
}
