using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using TPMS.Application.Features.Leases.Commands;
using TPMS.Application.Features.RenewLease.Commands;
using TPMS.Domain.Enums;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Leases.Handlers
{
    public class SettleLeaseTerminationHandler
        : IRequestHandler<SettleLeaseTerminationCommand, bool>
    {
        private readonly TPMSDBContext _db;

        public SettleLeaseTerminationHandler(TPMSDBContext db)
        {
            _db = db;
        }

        public async Task<bool> Handle(
            SettleLeaseTerminationCommand request,
            CancellationToken cancellationToken)
        {
            var termination = await _db.LeaseTerminations
                .FirstOrDefaultAsync(
                    t => t.LeaseTerminationID == request.LeaseTerminationID,
                    cancellationToken);

            if (termination == null)
                throw new InvalidOperationException("Termination not found.");

            if (termination.SettlementStatus ==   "Settled")
                throw new InvalidOperationException("Already settled.");

            termination.SettlementStatus = request.SettlementStatus;
            termination.SettledAt = DateTime.UtcNow;
            termination.SettledBy = request.ActionBy;

            await _db.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}