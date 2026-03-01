using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TPMS.Application.Features.Leases.Commands;
using TPMS.Application.Features.Leases.Services;

namespace TPMS.Application.Features.Leases.Handlers
{
    public class CompleteLeaseSettlementHandler 
        : IRequestHandler<CompleteLeaseSettlementCommand, Unit>
    {
        private readonly ILeaseSettlementService _settlementService;

        public CompleteLeaseSettlementHandler(
            ILeaseSettlementService settlementService)
        {
            _settlementService = settlementService;
        }

        public async Task<Unit> Handle(
            CompleteLeaseSettlementCommand request,
            CancellationToken cancellationToken)
        {
            await _settlementService.CompleteSettlementAsync(
                request.LeaseSettlementId,
                cancellationToken);

            return Unit.Value;
        }
    }
}