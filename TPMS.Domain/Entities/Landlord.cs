namespace TPMS.Domain.Entities
{
    public class Landlord
    {
        public int LandlordID { get; set; }
        public string? Name { get; set; }
        
        //  Back navigation
        public virtual ICollection<Property> Properties { get; set; }
            = new List<Property>();
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Soft delete flag
        public bool IsDeleted { get; set; } = false;
    }
}
