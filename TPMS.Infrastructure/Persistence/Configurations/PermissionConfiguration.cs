namespace TPMS.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TPMS.Domain.Entities;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.HasData(

            // ================= PROPERTY =================
            new Permission { PermissionID = 1001, PermissionName = "PROPERTY_CREATE", Description = "Create property", Module = "Property", IsSystem = true },
            new Permission { PermissionID = 1002, PermissionName = "PROPERTY_VIEW", Description = "View property", Module = "Property", IsSystem = true },
            new Permission { PermissionID = 1003, PermissionName = "PROPERTY_UPDATE", Description = "Update property", Module = "Property", IsSystem = true },
            new Permission { PermissionID = 1004, PermissionName = "PROPERTY_DELETE", Description = "Delete property", Module = "Property", IsSystem = true },
            new Permission { PermissionID = 1005, PermissionName = "PROPERTY_ARCHIVE", Description = "Archive property", Module = "Property", IsSystem = true },

            // ================= TENANT =================
            new Permission { PermissionID = 1101, PermissionName = "TENANT_CREATE", Description = "Create tenant", Module = "Tenant", IsSystem = true },
            new Permission { PermissionID = 1102, PermissionName = "TENANT_VIEW", Description = "View tenant", Module = "Tenant", IsSystem = true },
            new Permission { PermissionID = 1103, PermissionName = "TENANT_UPDATE", Description = "Update tenant", Module = "Tenant", IsSystem = true },
            new Permission { PermissionID = 1104, PermissionName = "TENANT_DELETE", Description = "Delete tenant", Module = "Tenant", IsSystem = true },
            new Permission { PermissionID = 1105, PermissionName = "TENANT_VERIFY_KYC", Description = "Verify tenant KYC", Module = "Tenant", IsSystem = true },

            // ================= LEASE =================
            new Permission { PermissionID = 1201, PermissionName = "LEASE_CREATE", Description = "Create lease", Module = "Lease", IsSystem = true },
            new Permission { PermissionID = 1202, PermissionName = "LEASE_VIEW", Description = "View lease", Module = "Lease", IsSystem = true },
            new Permission { PermissionID = 1203, PermissionName = "LEASE_UPDATE", Description = "Update lease", Module = "Lease", IsSystem = true },
            new Permission { PermissionID = 1204, PermissionName = "LEASE_APPROVE", Description = "Approve lease", Module = "Lease", IsSystem = true },
            new Permission { PermissionID = 1205, PermissionName = "LEASE_RENEW", Description = "Renew lease", Module = "Lease", IsSystem = true },
            new Permission { PermissionID = 1206, PermissionName = "LEASE_TERMINATE", Description = "Terminate lease", Module = "Lease", IsSystem = true },

            // ================= PAYMENT =================
            new Permission { PermissionID = 1301, PermissionName = "PAYMENT_RECORD_CREATE", Description = "Record payment", Module = "Payment", IsSystem = true },
            new Permission { PermissionID = 1302, PermissionName = "PAYMENT_VIEW", Description = "View payment", Module = "Payment", IsSystem = true },
            new Permission { PermissionID = 1303, PermissionName = "PAYMENT_EDIT", Description = "Edit payment", Module = "Payment", IsSystem = true },
            new Permission { PermissionID = 1304, PermissionName = "PAYMENT_REFUND", Description = "Refund payment", Module = "Payment", IsSystem = true },

            // ================= DEPOSIT =================
            new Permission { PermissionID = 1401, PermissionName = "DEPOSIT_COLLECT", Description = "Collect deposit", Module = "Deposit", IsSystem = true },
            new Permission { PermissionID = 1402, PermissionName = "DEPOSIT_VIEW", Description = "View deposit", Module = "Deposit", IsSystem = true },
            new Permission { PermissionID = 1403, PermissionName = "DEPOSIT_ADJUST", Description = "Adjust deposit", Module = "Deposit", IsSystem = true },
            new Permission { PermissionID = 1404, PermissionName = "DEPOSIT_REFUND", Description = "Refund deposit", Module = "Deposit", IsSystem = true },
            new Permission { PermissionID = 1499, PermissionName = "Test Permission", Description = "For testing the permission operations", Module = "Deposit", IsSystem = false },

            // ================= DOCUMENT =================
            new Permission { PermissionID = 1501, PermissionName = "DOCUMENT_UPLOAD", Description = "Upload document", Module = "Document", IsSystem = true },
            new Permission { PermissionID = 1502, PermissionName = "DOCUMENT_VIEW", Description = "View document", Module = "Document", IsSystem = true },
            new Permission { PermissionID = 1503, PermissionName = "DOCUMENT_DOWNLOAD", Description = "Download document", Module = "Document", IsSystem = true },
            new Permission { PermissionID = 1504, PermissionName = "DOCUMENT_DELETE", Description = "Delete document", Module = "Document", IsSystem = true },
            new Permission { PermissionID = 1505, PermissionName = "DOCUMENT_VERIFY", Description = "Verify document", Module = "Document", IsSystem = true },

            // Document Category
            new Permission { PermissionID = 1510, PermissionName = "DOCUMENT_CATEGORY_ADD", Description = "Permission to add document category", Module = "Document", IsSystem = true },
            new Permission { PermissionID = 1511, PermissionName = "DOCUMENT_CATEGORY_VIEW", Description = "Permission to view document category", Module = "Document", IsSystem = true },
            new Permission { PermissionID = 1512, PermissionName = "DOCUMENT_CATEGORY_EDIT", Description = "Permission to edit document category", Module = "Document", IsSystem = true },
            new Permission { PermissionID = 1513, PermissionName = "DOCUMENT_CATEGORY_DELETE", Description = "Permission to delete document category", Module = "Document", IsSystem = true },

            // Document Type
            new Permission { PermissionID = 1520, PermissionName = "DOCUMENT_TYPE_ADD", Description = "Permission to add document type", Module = "Document", IsSystem = true },
            new Permission { PermissionID = 1521, PermissionName = "DOCUMENT_TYPE_VIEW", Description = "Permission to view document types", Module = "Document", IsSystem = true },
            new Permission { PermissionID = 1522, PermissionName = "DOCUMENT_TYPE_EDIT", Description = "Permission to edit document type", Module = "Document", IsSystem = true },
            new Permission { PermissionID = 1523, PermissionName = "DOCUMENT_TYPE_DELETE", Description = "Permission to delete document type", Module = "Document", IsSystem = true },

            // ================= DISPUTE =================
            new Permission { PermissionID = 1601, PermissionName = "DISPUTE_CREATE", Description = "Create dispute", Module = "Dispute", IsSystem = true },
            new Permission { PermissionID = 1602, PermissionName = "DISPUTE_VIEW", Description = "View dispute", Module = "Dispute", IsSystem = true },
            new Permission { PermissionID = 1603, PermissionName = "DISPUTE_ASSIGN", Description = "Assign dispute", Module = "Dispute", IsSystem = true },
            new Permission { PermissionID = 1604, PermissionName = "DISPUTE_RESOLVE", Description = "Resolve dispute", Module = "Dispute", IsSystem = true },
            new Permission { PermissionID = 1605, PermissionName = "DISPUTE_CLOSE", Description = "Close dispute", Module = "Dispute", IsSystem = true },

            // ================= MAINTENANCE =================
            new Permission { PermissionID = 1701, PermissionName = "COMPLAINT_CREATE", Description = "Create complaint", Module = "Maintenance", IsSystem = true },
            new Permission { PermissionID = 1702, PermissionName = "COMPLAINT_VIEW", Description = "View complaint", Module = "Maintenance", IsSystem = true },
            new Permission { PermissionID = 1703, PermissionName = "COMPLAINT_ASSIGN", Description = "Assign complaint", Module = "Maintenance", IsSystem = true },
            new Permission { PermissionID = 1704, PermissionName = "COMPLAINT_UPDATE_STATUS", Description = "Update complaint status", Module = "Maintenance", IsSystem = true },
            new Permission { PermissionID = 1705, PermissionName = "COMPLAINT_CLOSE", Description = "Close complaint", Module = "Maintenance", IsSystem = true },

            // ================= USER =================
            new Permission { PermissionID = 1801, PermissionName = "USER_CREATE", Description = "Create user", Module = "User", IsSystem = true },
            new Permission { PermissionID = 1802, PermissionName = "USER_VIEW", Description = "View user", Module = "User", IsSystem = true },
            new Permission { PermissionID = 1803, PermissionName = "USER_UPDATE", Description = "Update user", Module = "User", IsSystem = true },
            new Permission { PermissionID = 1804, PermissionName = "USER_DELETE", Description = "Delete user", Module = "User", IsSystem = true },
            new Permission { PermissionID = 1805, PermissionName = "USER_ASSIGN_ROLE", Description = "Assign role to user", Module = "User", IsSystem = true },

            // ================= ROLE =================
            new Permission { PermissionID = 1901, PermissionName = "ROLE_CREATE", Description = "Create role", Module = "Role", IsSystem = true },
            new Permission { PermissionID = 1902, PermissionName = "ROLE_VIEW", Description = "View role", Module = "Role", IsSystem = true },
            new Permission { PermissionID = 1903, PermissionName = "ROLE_UPDATE", Description = "Update role", Module = "Role", IsSystem = true },
            new Permission { PermissionID = 1904, PermissionName = "ROLE_DELETE", Description = "Delete role", Module = "Role", IsSystem = true },
            new Permission { PermissionID = 1905, PermissionName = "ROLE_ASSIGN_PERMISSION", Description = "Assign permission to role", Module = "Role", IsSystem = true },

            // ================= REPORT =================
            new Permission { PermissionID = 2001, PermissionName = "REPORT_VIEW", Description = "View reports", Module = "Report", IsSystem = true },
            new Permission { PermissionID = 2002, PermissionName = "REPORT_GENERATE", Description = "Generate reports", Module = "Report", IsSystem = true },
            new Permission { PermissionID = 2003, PermissionName = "REPORT_EXPORT", Description = "Export reports", Module = "Report", IsSystem = true },

            // ================= LANDLORD =================
            new Permission { PermissionID = 2101, PermissionName = "LANDLORD_CREATE", Description = "Create landlord", Module = "Landlord", IsSystem = true },
            new Permission { PermissionID = 2102, PermissionName = "LANDLORD_VIEW", Description = "View landlord", Module = "Landlord", IsSystem = true },
            new Permission { PermissionID = 2103, PermissionName = "LANDLORD_UPDATE", Description = "Update landlord", Module = "Landlord", IsSystem = true },
            new Permission { PermissionID = 2104, PermissionName = "LANDLORD_DELETE", Description = "Delete landlord", Module = "Landlord", IsSystem = true },

            // ================= ASSET CATEGORY =================
            new Permission { PermissionID = 2201, PermissionName = "ASSET_CATEGORY_ADD", Description = "Permission to add asset category", Module = "Asset Category", IsSystem = true },
            new Permission { PermissionID = 2202, PermissionName = "ASSET_CATEGORY_VIEW", Description = "Permission to view asset category", Module = "Asset Category", IsSystem = true },
            new Permission { PermissionID = 2203, PermissionName = "ASSET_CATEGORY_EDIT", Description = "Permission to edit asset category", Module = "Asset Category", IsSystem = true }
        );
    }
}
