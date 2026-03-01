using System.ComponentModel.DataAnnotations.Schema;

namespace TPMS.Domain.Entities
{
    public class Tenant
    {
        public int TenantID { get; set; }
        
        public string TenantNumber { get; set; }
        public string? Name { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

       
        // Soft delete
        public bool IsDeleted { get; set; } = false;

        // Navigation Properties
        public virtual ICollection<Lease> Leases { get; set; } = new List<Lease>();

        // This relationship is optional if you use OwnerTypeID in Address table
        [NotMapped]
        public virtual ICollection<Address>? Addresses { get; set; }

    }
}
