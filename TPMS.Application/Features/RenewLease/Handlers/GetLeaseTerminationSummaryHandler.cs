using System.Collections.Generic;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Features.Leases.DTOs;
using TPMS.Application.Features.Leases.Queries;
using TPMS.Application.Features.RenewLease.DTOs;
using TPMS.Application.Features.RenewLease.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Leases.Handlers
{
    public class GetLeaseTerminationSummaryHandler
        : IRequestHandler<GetLeaseTerminationSummaryQuery, LeaseTerminationSummaryDto>
    {
        private readonly TPMSDBContext _db;

        public GetLeaseTerminationSummaryHandler(TPMSDBContext db)
        {
            _db = db;
        }

        public async Task<LeaseTerminationSummaryDto> Handle(
            GetLeaseTerminationSummaryQuery request,
            CancellationToken cancellationToken)
        {
            var termination = await _db.LeaseTerminations
                .Include(t => t.Lease)
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    t => t.LeaseTerminationID == request.LeaseTerminationID,
                    cancellationToken);

            if (termination == null)
                throw new KeyNotFoundException("Lease termination not found.");

            return new LeaseTerminationSummaryDto
            {
                LeaseTerminationID = termination.LeaseTerminationID,
                LeaseID = termination.LeaseID,

                TerminationDate = termination.TerminationDate,
                EffectiveEndDate = termination.EffectiveEndDate,
                TerminationType = termination.TerminationType,
                TerminationReason = termination.TerminationReason,

                OutstandingRent = termination.OutstandingRent,
                PenaltyAmount = termination.PenaltyAmount,
                DamageCharges = termination.DamageCharges,

                DepositAdjusted = termination.DepositAdjusted,
                DepositRefunded = termination.DepositRefunded,

                SettlementStatus = termination.SettlementStatus,

                LeaseName = termination.Lease.LeaseName,
                Rent = termination.Lease.Rent,
                Deposit = termination.Lease.Deposit,

                CreatedAt = termination.CreatedAt,
                CreatedBy = termination.CreatedBy
            };
        }
    }
}
