using System;

namespace TPMS.Domain.Entities
{
    public class RentSchedule
    {
        public int ScheduleID { get; set; }
        public int LeaseID { get; set; }
        public DateTime DueDate { get; set; }
        public decimal Amount { get; set; }
        public string? Status { get; set; }
        public bool IsPaid { get; set; } = false;
       
        public bool IsClosed { get; set; } = false;
        public DateTime? PaidDate { get; set; }
        public decimal? Penalty { get; set; }
      
        public bool IsDeleted { get; set; } = false;
        public virtual Lease? Lease { get; set; }

    }
}
