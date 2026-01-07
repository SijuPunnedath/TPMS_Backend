namespace TPMS.Domain.Entities
{
    public class Property
    {
        public int PropertyID { get; set; }
        
        public string PropertyName { get; set; } = string.Empty;

        public string? SerialNo { get; set; }
        public string? Type { get; set; }
        public string? Size { get; set; }
        public int? LandlordID { get; set; }
        
        //  Navigation
        public virtual Landlord? Landlord { get; set; }
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Soft delete
        public bool IsDeleted { get; set; } = false;

    }
}
