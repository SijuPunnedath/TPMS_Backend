using MediatR;
using TPMS.Application.Features.Leases.DTOs;

namespace TPMS.Application.Features.Leases.Commands
{
    public class CreateLeaseCommand : IRequest<int>
    {
        public LeaseCreateDto Lease { get; }

        public CreateLeaseCommand(LeaseCreateDto lease)
        {
            Lease = lease;
        }
    }
}