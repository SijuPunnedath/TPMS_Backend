using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMS.Application.Features.LeaseAlert.DTOs
{
    public class LeaseAlertDto
    {
        public int AlertID { get; set; }
        public int LeaseID { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime AlertDate { get; set; }
        //public bool IsResolved { get; set; }
        public string Status { get; set; } = "Pending";
    }
}
