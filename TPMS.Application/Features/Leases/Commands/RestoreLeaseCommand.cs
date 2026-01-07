using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMS.Application.Features.Leases.Commands
{
    public class RestoreLeaseCommand : IRequest<bool>
    {
        public int LeaseId { get; }
        public RestoreLeaseCommand(int leaseId) => LeaseId = leaseId;
    }
}
