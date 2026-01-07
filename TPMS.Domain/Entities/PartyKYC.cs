using System;

namespace TPMS.Domain.Entities
{
    public class PartyKYC
    {
        public int PartyKYCID { get; set; }
        public int OwnerTypeID { get; set; }
        public int OwnerID { get; set; }
        public int KYCTypeID { get; set; }
        public int? DocumentID { get; set; }
        public string? KYCNumber { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? Status { get; set; }
        public int? VerifiedBy { get; set; }
        public DateTime? VerifiedAt { get; set; }
    }
}
