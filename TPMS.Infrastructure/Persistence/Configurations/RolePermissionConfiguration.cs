using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TPMS.Domain.Entities;

namespace TPMS.Infrastructure.Persistence.Configurations
{
    public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
           

            const int ADMIN_ROLE_ID = 1;

            builder.HasData(
                // ================= PROPERTY =================
                new RolePermission { RolePermissionID = 1, RoleID = ADMIN_ROLE_ID, PermissionID = 1001 },
                new RolePermission { RolePermissionID = 2, RoleID = ADMIN_ROLE_ID, PermissionID = 1002 },
                new RolePermission { RolePermissionID = 3, RoleID = ADMIN_ROLE_ID, PermissionID = 1003 },
                new RolePermission { RolePermissionID = 4, RoleID = ADMIN_ROLE_ID, PermissionID = 1004 },
                new RolePermission { RolePermissionID = 5, RoleID = ADMIN_ROLE_ID, PermissionID = 1005 },

                // ================= TENANT =================
                new RolePermission { RolePermissionID = 6, RoleID = ADMIN_ROLE_ID, PermissionID = 1101 },
                new RolePermission { RolePermissionID = 7, RoleID = ADMIN_ROLE_ID, PermissionID = 1102 },
                new RolePermission { RolePermissionID = 8, RoleID = ADMIN_ROLE_ID, PermissionID = 1103 },
                new RolePermission { RolePermissionID = 9, RoleID = ADMIN_ROLE_ID, PermissionID = 1104 },
                new RolePermission { RolePermissionID = 10, RoleID = ADMIN_ROLE_ID, PermissionID = 1105 },

                // ================= LEASE =================
                new RolePermission { RolePermissionID = 11, RoleID = ADMIN_ROLE_ID, PermissionID = 1201 },
                new RolePermission { RolePermissionID = 12, RoleID = ADMIN_ROLE_ID, PermissionID = 1202 },
                new RolePermission { RolePermissionID = 13, RoleID = ADMIN_ROLE_ID, PermissionID = 1203 },
                new RolePermission { RolePermissionID = 14, RoleID = ADMIN_ROLE_ID, PermissionID = 1204 },
                new RolePermission { RolePermissionID = 15, RoleID = ADMIN_ROLE_ID, PermissionID = 1205 },
                new RolePermission { RolePermissionID = 16, RoleID = ADMIN_ROLE_ID, PermissionID = 1206 },

                // ================= PAYMENT =================
                new RolePermission { RolePermissionID = 17, RoleID = ADMIN_ROLE_ID, PermissionID = 1301 },
                new RolePermission { RolePermissionID = 18, RoleID = ADMIN_ROLE_ID, PermissionID = 1302 },
                new RolePermission { RolePermissionID = 19, RoleID = ADMIN_ROLE_ID, PermissionID = 1303 },
                new RolePermission { RolePermissionID = 20, RoleID = ADMIN_ROLE_ID, PermissionID = 1304 },

                // ================= DEPOSIT =================
                new RolePermission { RolePermissionID = 21, RoleID = ADMIN_ROLE_ID, PermissionID = 1401 },
                new RolePermission { RolePermissionID = 22, RoleID = ADMIN_ROLE_ID, PermissionID = 1402 },
                new RolePermission { RolePermissionID = 23, RoleID = ADMIN_ROLE_ID, PermissionID = 1403 },
                new RolePermission { RolePermissionID = 24, RoleID = ADMIN_ROLE_ID, PermissionID = 1404 },
                new RolePermission { RolePermissionID = 25, RoleID = ADMIN_ROLE_ID, PermissionID = 1499 },

                // ================= DOCUMENT =================
                new RolePermission { RolePermissionID = 26, RoleID = ADMIN_ROLE_ID, PermissionID = 1501 },
                new RolePermission { RolePermissionID = 27, RoleID = ADMIN_ROLE_ID, PermissionID = 1502 },
                new RolePermission { RolePermissionID = 28, RoleID = ADMIN_ROLE_ID, PermissionID = 1503 },
                new RolePermission { RolePermissionID = 29, RoleID = ADMIN_ROLE_ID, PermissionID = 1504 },
                new RolePermission { RolePermissionID = 30, RoleID = ADMIN_ROLE_ID, PermissionID = 1505 },

                // Document Category
                new RolePermission { RolePermissionID = 31, RoleID = ADMIN_ROLE_ID, PermissionID = 1510 },
                new RolePermission { RolePermissionID = 32, RoleID = ADMIN_ROLE_ID, PermissionID = 1511 },
                new RolePermission { RolePermissionID = 33, RoleID = ADMIN_ROLE_ID, PermissionID = 1512 },
                new RolePermission { RolePermissionID = 34, RoleID = ADMIN_ROLE_ID, PermissionID = 1513 },

                // Document Type
                new RolePermission { RolePermissionID = 35, RoleID = ADMIN_ROLE_ID, PermissionID = 1520 },
                new RolePermission { RolePermissionID = 36, RoleID = ADMIN_ROLE_ID, PermissionID = 1521 },
                new RolePermission { RolePermissionID = 37, RoleID = ADMIN_ROLE_ID, PermissionID = 1522 },
                new RolePermission { RolePermissionID = 38, RoleID = ADMIN_ROLE_ID, PermissionID = 1523 },

                // ================= DISPUTE =================
                new RolePermission { RolePermissionID = 39, RoleID = ADMIN_ROLE_ID, PermissionID = 1601 },
                new RolePermission { RolePermissionID = 40, RoleID = ADMIN_ROLE_ID, PermissionID = 1602 },
                new RolePermission { RolePermissionID = 41, RoleID = ADMIN_ROLE_ID, PermissionID = 1603 },
                new RolePermission { RolePermissionID = 42, RoleID = ADMIN_ROLE_ID, PermissionID = 1604 },
                new RolePermission { RolePermissionID = 43, RoleID = ADMIN_ROLE_ID, PermissionID = 1605 },

                // ================= MAINTENANCE =================
                new RolePermission { RolePermissionID = 44, RoleID = ADMIN_ROLE_ID, PermissionID = 1701 },
                new RolePermission { RolePermissionID = 45, RoleID = ADMIN_ROLE_ID, PermissionID = 1702 },
                new RolePermission { RolePermissionID = 46, RoleID = ADMIN_ROLE_ID, PermissionID = 1703 },
                new RolePermission { RolePermissionID = 47, RoleID = ADMIN_ROLE_ID, PermissionID = 1704 },
                new RolePermission { RolePermissionID = 48, RoleID = ADMIN_ROLE_ID, PermissionID = 1705 },

                // ================= USER =================
                new RolePermission { RolePermissionID = 49, RoleID = ADMIN_ROLE_ID, PermissionID = 1801 },
                new RolePermission { RolePermissionID = 50, RoleID = ADMIN_ROLE_ID, PermissionID = 1802 },
                new RolePermission { RolePermissionID = 51, RoleID = ADMIN_ROLE_ID, PermissionID = 1803 },
                new RolePermission { RolePermissionID = 52, RoleID = ADMIN_ROLE_ID, PermissionID = 1804 },
                new RolePermission { RolePermissionID = 53, RoleID = ADMIN_ROLE_ID, PermissionID = 1805 },

                // ================= ROLE =================
                new RolePermission { RolePermissionID = 54, RoleID = ADMIN_ROLE_ID, PermissionID = 1901 },
                new RolePermission { RolePermissionID = 55, RoleID = ADMIN_ROLE_ID, PermissionID = 1902 },
                new RolePermission { RolePermissionID = 56, RoleID = ADMIN_ROLE_ID, PermissionID = 1903 },
                new RolePermission { RolePermissionID = 57, RoleID = ADMIN_ROLE_ID, PermissionID = 1904 },
                new RolePermission { RolePermissionID = 58, RoleID = ADMIN_ROLE_ID, PermissionID = 1905 },

                // ================= REPORT =================
                new RolePermission { RolePermissionID = 59, RoleID = ADMIN_ROLE_ID, PermissionID = 2001 },
                new RolePermission { RolePermissionID = 60, RoleID = ADMIN_ROLE_ID, PermissionID = 2002 },
                new RolePermission { RolePermissionID = 61, RoleID = ADMIN_ROLE_ID, PermissionID = 2003 },

                // ================= LANDLORD =================
                new RolePermission { RolePermissionID = 62, RoleID = ADMIN_ROLE_ID, PermissionID = 2101 },
                new RolePermission { RolePermissionID = 63, RoleID = ADMIN_ROLE_ID, PermissionID = 2102 },
                new RolePermission { RolePermissionID = 64, RoleID = ADMIN_ROLE_ID, PermissionID = 2103 },
                new RolePermission { RolePermissionID = 65, RoleID = ADMIN_ROLE_ID, PermissionID = 2104 },

                // ================= ASSET CATEGORY =================
                new RolePermission { RolePermissionID = 66, RoleID = ADMIN_ROLE_ID, PermissionID = 2201 },
                new RolePermission { RolePermissionID = 67, RoleID = ADMIN_ROLE_ID, PermissionID = 2202 },
                new RolePermission { RolePermissionID = 68, RoleID = ADMIN_ROLE_ID, PermissionID = 2203 }
            );
        }
    }
}