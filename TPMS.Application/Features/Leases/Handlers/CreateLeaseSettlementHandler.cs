using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TPMS.Application.Features.Leases.Commands;
using TPMS.Application.Features.Leases.DTOs;
using TPMS.Application.Features.Leases.Services;

namespace TPMS.Application.Features.Leases.Handlers
{
    public class CreateLeaseSettlementHandler 
        : IRequestHandler<CreateLeaseSettlementCommand, LeaseSettlementDto>
    {
        private readonly ILeaseSettlementService _settlementService;

        public CreateLeaseSettlementHandler(
            ILeaseSettlementService settlementService)
        {
            _settlementService = settlementService;
        }

        public async Task<LeaseSettlementDto> Handle(
            CreateLeaseSettlementCommand request,
            CancellationToken cancellationToken)
        {
            var settlement = await _settlementService.CreateSettlementAsync(
                request.LeaseId,
                request.PenaltyAmount,
                request.DamageCharges,
                cancellationToken);

            return new LeaseSettlementDto
            {
                LeaseSettlementId = settlement.LeaseSettlementId,
                LeaseId = settlement.LeaseId,
                OutstandingRent = settlement.OutstandingRent,
                DepositPaid = settlement.DepositPaid,
                DepositAdjusted = settlement.DepositAdjusted,
                DepositRefunded = settlement.DepositRefunded,
                BalancePayableByTenant = settlement.BalancePayableByTenant,
                Status = settlement.Status.ToString(),
                SettlementDate = settlement.SettlementDate
            };
        }
    }
}