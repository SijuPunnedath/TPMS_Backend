using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMS.Application.Features.Leases.Commands
{
    public class DeleteLeaseCommand : IRequest<bool>
    {
        public int LeaseId { get; }
        public DeleteLeaseCommand(int leaseId) => LeaseId = leaseId;
    }
}
