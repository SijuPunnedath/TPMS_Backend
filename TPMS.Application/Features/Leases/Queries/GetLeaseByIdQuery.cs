using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMS.Application.Features.Leases.DTOs;

namespace TPMS.Application.Features.Leases.Queries
{
    public class GetLeaseByIdQuery : IRequest<LeaseDto?>
    {
        public int LeaseId { get; }
        public GetLeaseByIdQuery(int leaseId) => LeaseId = leaseId;
    }
}
