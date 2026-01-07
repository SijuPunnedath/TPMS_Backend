using MediatR;

namespace TPMS.Application.Features.RenewLease.Commands;

public record SettleLeaseTerminationCommand(
   int LeaseTerminationID,
   string SettlementStatus,   // Settled | Disputed
   int ActionBy,
   string? Notes
) : IRequest<bool>;