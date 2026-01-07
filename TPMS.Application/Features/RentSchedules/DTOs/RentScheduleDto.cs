using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMS.Domain.Entities;

namespace TPMS.Application.Features.RentSchedules.DTOs
{
    public class RentScheduleDto
    {
        public int ScheduleID { get; set; }
        public int LeaseID { get; set; }

        public DateTime DueDate { get; set; }
        public decimal Amount { get; set; }
        public bool IsPaid { get; set; }
        public DateTime? PaidDate { get; set; }
        public decimal? Penalty { get; set; }

        public virtual Lease Lease { get; set; } = null!;
    }
}
