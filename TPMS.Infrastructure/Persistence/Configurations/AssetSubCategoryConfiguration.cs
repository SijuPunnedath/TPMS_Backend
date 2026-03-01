using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TPMS.Domain.Entities;

namespace TPMS.Infrastructure.Persistence.Configurations
{
    public class AssetSubCategoryConfiguration : IEntityTypeConfiguration<AssetSubCategory>
    {
        public void Configure(EntityTypeBuilder<AssetSubCategory> builder)
        {
          //  builder.ToTable("AssetSubCategories");

           // builder.HasKey(x => x.AssetSubCategoryId);

           // builder.Property(x => x.Name)
                  // .IsRequired()
                  // .HasMaxLength(150);

           // builder.HasOne(x => x.AssetCategory)
                //   .WithMany()
                //   .HasForeignKey(x => x.AssetCategoryId);

            builder.HasData(

                // ================= SAFETY & SECURITY (1) =================
                new AssetSubCategory { AssetSubCategoryId = 1001, AssetCategoryId = 1, Name = "Fire Extinguishers", IsActive = true },
                new AssetSubCategory { AssetSubCategoryId = 1002, AssetCategoryId = 1, Name = "Fire Alarm System", IsActive = true },
                new AssetSubCategory { AssetSubCategoryId = 1003, AssetCategoryId = 1, Name = "Smoke Detectors", IsActive = true },
                new AssetSubCategory { AssetSubCategoryId = 1004, AssetCategoryId = 1, Name = "CCTV Cameras", IsActive = true },
                new AssetSubCategory { AssetSubCategoryId = 1005, AssetCategoryId = 1, Name = "Access Control System", IsActive = true },

                // ================= PLUMBING & SANITARY (2) =================
                new AssetSubCategory { AssetSubCategoryId = 1101, AssetCategoryId = 2, Name = "Water Supply Pipes", IsActive = true },
                new AssetSubCategory { AssetSubCategoryId = 1102, AssetCategoryId = 2, Name = "Drainage / Sewer Lines", IsActive = true },
                new AssetSubCategory { AssetSubCategoryId = 1103, AssetCategoryId = 2, Name = "Toilets & WC Units", IsActive = true },
                new AssetSubCategory { AssetSubCategoryId = 1104, AssetCategoryId = 2, Name = "Wash Basins", IsActive = true },
                new AssetSubCategory { AssetSubCategoryId = 1105, AssetCategoryId = 2, Name = "Water Tanks", IsActive = true },

                // ================= MECHANICAL (3) =================
                new AssetSubCategory { AssetSubCategoryId = 1201, AssetCategoryId = 3, Name = "Elevators / Lifts", IsActive = true },
                new AssetSubCategory { AssetSubCategoryId = 1202, AssetCategoryId = 3, Name = "HVAC Systems", IsActive = true },
                new AssetSubCategory { AssetSubCategoryId = 1203, AssetCategoryId = 3, Name = "Ventilation Systems", IsActive = true },
                new AssetSubCategory { AssetSubCategoryId = 1204, AssetCategoryId = 3, Name = "Generators", IsActive = true },

                // ================= STRUCTURAL (4) =================
                new AssetSubCategory { AssetSubCategoryId = 1301, AssetCategoryId = 4, Name = "Foundation", IsActive = true },
                new AssetSubCategory { AssetSubCategoryId = 1302, AssetCategoryId = 4, Name = "Columns & Beams", IsActive = true },
                new AssetSubCategory { AssetSubCategoryId = 1303, AssetCategoryId = 4, Name = "Roof Structure", IsActive = true },

                // ================= UTILITY & METERING (5) =================
                new AssetSubCategory { AssetSubCategoryId = 1401, AssetCategoryId = 5, Name = "Electricity Meter", IsActive = true },
                new AssetSubCategory { AssetSubCategoryId = 1402, AssetCategoryId = 5, Name = "Water Meter", IsActive = true },
                new AssetSubCategory { AssetSubCategoryId = 1403, AssetCategoryId = 5, Name = "Gas Meter", IsActive = true },

                // ================= ELECTRICAL (7) =================
                new AssetSubCategory { AssetSubCategoryId = 1501, AssetCategoryId = 6, Name = "Internal Wiring", IsActive = true },
                new AssetSubCategory { AssetSubCategoryId = 1502, AssetCategoryId = 6, Name = "Distribution Boards", IsActive = true },
                new AssetSubCategory { AssetSubCategoryId = 1503, AssetCategoryId = 6, Name = "Lighting Fixtures", IsActive = true },

                // ================= FURNITURE (10) =================
                new AssetSubCategory { AssetSubCategoryId = 1601, AssetCategoryId = 7, Name = "Chairs", IsActive = true },
                new AssetSubCategory { AssetSubCategoryId = 1602, AssetCategoryId = 7, Name = "Tables", IsActive = true },
                new AssetSubCategory { AssetSubCategoryId = 1603, AssetCategoryId = 7, Name = "Wardrobes / Cupboards", IsActive = true },

                // ================= APPLIANCES (11) =================
                new AssetSubCategory { AssetSubCategoryId = 1701, AssetCategoryId = 8, Name = "Air Conditioner", IsActive = true },
                new AssetSubCategory { AssetSubCategoryId = 1702, AssetCategoryId = 8, Name = "Refrigerator", IsActive = true },
                new AssetSubCategory { AssetSubCategoryId = 1703, AssetCategoryId = 8, Name = "Washing Machine", IsActive = true },

                // ================= KITCHEN (12) =================
                new AssetSubCategory { AssetSubCategoryId = 1801, AssetCategoryId = 9, Name = "Kitchen Cabinets", IsActive = true },
                new AssetSubCategory { AssetSubCategoryId = 1802, AssetCategoryId = 9, Name = "Kitchen Sink", IsActive = true },
                new AssetSubCategory { AssetSubCategoryId = 1803, AssetCategoryId = 9, Name = "Gas Stove", IsActive = true },

                // ================= DOCUMENTS & INVENTORY (13) =================
                new AssetSubCategory { AssetSubCategoryId = 1901, AssetCategoryId = 10, Name = "Property Documents", IsActive = true },
                new AssetSubCategory { AssetSubCategoryId = 1902, AssetCategoryId = 10, Name = "Lease Agreements", IsActive = true }
            );
        }
    }
}
