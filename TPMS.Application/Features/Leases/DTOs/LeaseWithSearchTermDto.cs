using System;
using System.Collections.Generic;
using TPMS.Application.Features.Addresses.DTOs;
using TPMS.Application.Features.RentSchedules.DTOs;
using TPMS.Domain.Enums;

namespace TPMS.Application.Features.Leases.DTOs
{
    public class LeaseWithSearchTermDto
    {
        public int LeaseID { get; set; }
        public int PropertyID { get; set; }

        public int? TenantID { get; set; }
        public int? LandlordID { get; set; }

        public string TenantName { get; set; } = string.Empty;
        public string LandlordName { get; set; } = string.Empty;
        public string PropertyName { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        
        public DateTime? DateMovedIn { get; set; }
        public decimal Rent { get; set; }
        public decimal Deposit { get; set; }

        public string Status { get; set; } = "Active";
        public string PaymentFrequency { get; set; } = "Monthly";

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // ✅ Enum
        public LeaseType LeaseType { get; set; }
        public string LeaseName { get; set; }
        
        public string LeaseNotes  { get; set; }
        public AddressDto? PropertyAddress { get; set; }
        public AddressDto? TenantAddress { get; set; }
        public AddressDto? LandlordAddress { get; set; }
        public decimal? Commission { get; set; }
        public List<RentScheduleDto> RentSchedules { get; set; } = new();
    }
}