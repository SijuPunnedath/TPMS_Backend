using MediatR;

namespace TPMS.Application.Features.Leases.Commands;

public class CompleteLeaseSettlementCommand : IRequest<Unit>
{
    public int LeaseSettlementId { get; set; }
}