using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMS.Application.Features.LeaseAlert.DTOs;
using TPMS.Application.Features.RentSchedules.DTOs;
using TPMS.Domain.Enums;

namespace TPMS.Application.Features.Leases.DTOs
{
    public class LeaseWithScheduleAlertsDto
    {
        public int LeaseID { get; set; }
        
        public string LeaseNumber { get; set; }
        public string LeaseName { get; set; } = string.Empty;
        public int PropertyID { get; set; }
        
        public string PropertyNumber { get; set; } = string.Empty;
        public int? TenantID { get; set; }
        public int? LandlordID { get; set; }
        public int? ParentLeaseID { get; set; } // For Renewal 
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        
        public DateTime? DateMovedIn { get; set; }
        public decimal Rent { get; set; }
        public decimal Deposit { get; set; }
        //public string Status { get; set; } = "Active";
        public LeaseStatus? Status { get; set; } = LeaseStatus.Active;
        public string PaymentFrequency { get; set; } = "Monthly";
        public int? PenaltyPolicyID { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<RentScheduleDto> RentSchedules { get; set; } = new();
        public List<LeaseAlertDto> LeaseAlerts { get; set; } = new();
    }
}
