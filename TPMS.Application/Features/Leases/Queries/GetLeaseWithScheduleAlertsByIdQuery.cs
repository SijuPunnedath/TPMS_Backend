using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMS.Application.Features.Leases.DTOs;

namespace TPMS.Application.Features.Leases.Queries
{
    public class GetLeaseWithScheduleAlertsByIdQuery :  IRequest<LeaseWithScheduleAlertsDto>
    {

        public int LeaseId { get; }
        public GetLeaseWithScheduleAlertsByIdQuery(int leaseId) => LeaseId = leaseId;   

    }
}
