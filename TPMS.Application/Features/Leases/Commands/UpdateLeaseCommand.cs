using MediatR;
using TPMS.Application.Features.Leases.DTOs;

namespace TPMS.Application.Features.Leases.Commands
{
    public class UpdateLeaseCommand : IRequest<bool>
    {
       // public int LeaseId { get; }
        public LeaseUpdateDto Lease { get; }

        public UpdateLeaseCommand(LeaseUpdateDto lease)
        {
           // LeaseId = leaseId;
            Lease = lease;
        }
    }
}