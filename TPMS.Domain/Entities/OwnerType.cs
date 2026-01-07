namespace TPMS.Domain.Entities
{
    public class OwnerType
    {
        public int OwnerTypeID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        
        // Soft delete flag
        public bool IsDeleted { get; set; } = false;

        // Audit fields
        public DateTime CreatedAt { get; set; } 
        public DateTime? UpdatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
