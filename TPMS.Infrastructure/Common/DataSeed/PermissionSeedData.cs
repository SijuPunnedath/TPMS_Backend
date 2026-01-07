using TPMS.Domain.Entities;

namespace TPMS.Infrastructure.Common.DataSeed;

public static class PermissionSeedData
{
    public static List<Permission> GetPermissions() => new()
    {
        // Property
        New("PROPERTY_CREATE", "Create property", "Property"),
        New("PROPERTY_VIEW", "View property", "Property"),
        New("PROPERTY_UPDATE", "Update property", "Property"),
        New("PROPERTY_DELETE", "Delete property", "Property"),
        New("PROPERTY_ARCHIVE", "Archive property", "Property"),

        // Tenant
        New("TENANT_CREATE", "Create tenant", "Tenant"),
        New("TENANT_VIEW", "View tenant", "Tenant"),
        New("TENANT_UPDATE", "Update tenant", "Tenant"),
        New("TENANT_DELETE", "Delete tenant", "Tenant"),
        New("TENANT_VERIFY_KYC", "Verify tenant KYC", "Tenant"),

        // Lease
        New("LEASE_CREATE", "Create lease", "Lease"),
        New("LEASE_VIEW", "View lease", "Lease"),
        New("LEASE_UPDATE", "Update lease", "Lease"),
        New("LEASE_APPROVE", "Approve lease", "Lease"),
        New("LEASE_RENEW", "Renew lease", "Lease"),
        New("LEASE_TERMINATE", "Terminate lease", "Lease"),

        // Payment
        New("PAYMENT_RECORD_CREATE", "Record payment", "Payment"),
        New("PAYMENT_VIEW", "View payment", "Payment"),
        New("PAYMENT_EDIT", "Edit payment", "Payment"),
        New("PAYMENT_REFUND", "Refund payment", "Payment"),

        // Deposit
        New("DEPOSIT_COLLECT", "Collect deposit", "Deposit"),
        New("DEPOSIT_VIEW", "View deposit", "Deposit"),
        New("DEPOSIT_ADJUST", "Adjust deposit", "Deposit"),
        New("DEPOSIT_REFUND", "Refund deposit", "Deposit"),

        // Document
        New("DOCUMENT_UPLOAD", "Upload document", "Document"),
        New("DOCUMENT_VIEW", "View document", "Document"),
        New("DOCUMENT_DOWNLOAD", "Download document", "Document"),
        New("DOCUMENT_DELETE", "Delete document", "Document"),
        New("DOCUMENT_VERIFY", "Verify document", "Document"),

        // Dispute
        New("DISPUTE_CREATE", "Create dispute", "Dispute"),
        New("DISPUTE_VIEW", "View dispute", "Dispute"),
        New("DISPUTE_ASSIGN", "Assign dispute", "Dispute"),
        New("DISPUTE_RESOLVE", "Resolve dispute", "Dispute"),
        New("DISPUTE_CLOSE", "Close dispute", "Dispute"),

        // Maintenance
        New("COMPLAINT_CREATE", "Create complaint", "Maintenance"),
        New("COMPLAINT_VIEW", "View complaint", "Maintenance"),
        New("COMPLAINT_ASSIGN", "Assign complaint", "Maintenance"),
        New("COMPLAINT_UPDATE_STATUS", "Update complaint status", "Maintenance"),
        New("COMPLAINT_CLOSE", "Close complaint", "Maintenance"),

        // User & Role
        New("USER_CREATE", "Create user", "User"),
        New("USER_VIEW", "View user", "User"),
        New("USER_UPDATE", "Update user", "User"),
        New("USER_DELETE", "Delete user", "User"),
        New("USER_ASSIGN_ROLE", "Assign role to user", "User"),

        New("ROLE_CREATE", "Create role", "Role"),
        New("ROLE_VIEW", "View role", "Role"),
        New("ROLE_UPDATE", "Update role", "Role"),
        New("ROLE_DELETE", "Delete role", "Role"),
        New("ROLE_ASSIGN_PERMISSION", "Assign permission to role", "Role"),

        // Reports
        New("REPORT_VIEW", "View reports", "Report"),
        New("REPORT_GENERATE", "Generate reports", "Report"),
        New("REPORT_EXPORT", "Export reports", "Report")
    };

    private static Permission New(string name, string description, string module) =>
        new Permission
        {
            PermissionName = name,
            Description = description,
            Module = module,
            IsSystem = true
        };
}
