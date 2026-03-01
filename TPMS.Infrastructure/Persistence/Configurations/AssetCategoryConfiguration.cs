namespace TPMS.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TPMS.Domain.Entities;

public class AssetCategoryConfiguration : IEntityTypeConfiguration<AssetCategory>
{
    public void Configure(EntityTypeBuilder<AssetCategory> builder)
    {
        builder.ToTable("AssetCategories");

        builder.HasKey(x => x.AssetCategoryId);

        builder.Property(x => x.AssetCategoryId)
               .ValueGeneratedOnAdd();

        builder.Property(x => x.CategoryName)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(x => x.Code)
               .IsRequired()
               .HasMaxLength(50);

        builder.HasIndex(x => x.Code)
               .IsUnique();

        builder.Property(x => x.IsDepreciable)
               .HasDefaultValue(false);

        builder.Property(x => x.DefaultUsefulLifeMonths)
               .HasDefaultValue(0);

        builder.Property(x => x.RequiresComplianceCheck)
               .HasDefaultValue(false);

        builder.Property(x => x.IsActive)
               .HasDefaultValue(true);

        //  Seed data (fixed IDs – DO NOT CHANGE ONCE APPLIED)
        builder.HasData(
            new AssetCategory
            {
                AssetCategoryId = 1,
                CategoryName = "Safety & Security",
                Code = "SEC",
                IsDepreciable = false,
                DefaultUsefulLifeMonths = 0,
                RequiresComplianceCheck = true,
                IsActive = true
            },
            new AssetCategory
            {
                AssetCategoryId = 2,
                CategoryName = "Plumbing & Sanitary",
                Code = "PLUMB",
                IsDepreciable = true,
                DefaultUsefulLifeMonths = 120,
                RequiresComplianceCheck = false,
                IsActive = true
            },
            new AssetCategory
            {
                AssetCategoryId = 3,
                CategoryName = "Mechanical",
                Code = "MECH",
                IsDepreciable = true,
                DefaultUsefulLifeMonths = 120,
                RequiresComplianceCheck = false,
                IsActive = true
            },
            new AssetCategory
            {
                AssetCategoryId = 4,
                CategoryName = "Structural",
                Code = "STRUCT",
                IsDepreciable = true,
                DefaultUsefulLifeMonths = 300,
                RequiresComplianceCheck = true,
                IsActive = true
            },
            new AssetCategory
            {
                AssetCategoryId = 5,
                CategoryName = "Utility & Metering",
                Code = "UTIL",
                IsDepreciable = true,
                DefaultUsefulLifeMonths = 120,
                RequiresComplianceCheck = true,
                IsActive = true
            },
            new AssetCategory
            {
                AssetCategoryId = 6,
                CategoryName = "Electrical",
                Code = "ELEC",
                IsDepreciable = true,
                DefaultUsefulLifeMonths = 120,
                RequiresComplianceCheck = true,
                IsActive = true
            },
            new AssetCategory
            {
                AssetCategoryId = 7,
                CategoryName = "Furniture",
                Code = "FURN",
                IsDepreciable = true,
                DefaultUsefulLifeMonths = 60,
                RequiresComplianceCheck = false,
                IsActive = true
            },
            new AssetCategory
            {
                AssetCategoryId = 8,
                CategoryName = "Appliances",
                Code = "APPL",
                IsDepreciable = true,
                DefaultUsefulLifeMonths = 60,
                RequiresComplianceCheck = false,
                IsActive = true
            },
            new AssetCategory
            {
                AssetCategoryId = 9,
                CategoryName = "Kitchen",
                Code = "KITCH",
                IsDepreciable = true,
                DefaultUsefulLifeMonths = 60,
                RequiresComplianceCheck = false,
                IsActive = true
            },
            new AssetCategory
            {
                AssetCategoryId = 10,
                CategoryName = "Documents & Inventory",
                Code = "DOCINV",
                IsDepreciable = false,
                DefaultUsefulLifeMonths = 0,
                RequiresComplianceCheck = true,
                IsActive = true
            }
        );
    }
}
