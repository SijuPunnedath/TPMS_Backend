using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.Leases.Commands;
using TPMS.Domain.Enums;
using TPMS.Domain.Guards;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Leases.Handlers
{
    public class UpdateLeaseHandler : IRequestHandler<UpdateLeaseCommand, bool>
    {
        private readonly TPMSDBContext _db;

        public UpdateLeaseHandler(TPMSDBContext db)
        {
            _db = db;
        }

        public async Task<bool> Handle(UpdateLeaseCommand request, CancellationToken cancellationToken)
        {
            var lease = await _db.Leases
                .Include(l => l.DepositMaster)
                .FirstOrDefaultAsync(l => l.LeaseID == request.Lease.LeaseID && !l.IsDeleted, cancellationToken);

            if (lease == null) return false;

            // Apply updates
            lease.LeaseName = request.Lease.LeaseName;
            lease.PropertyID = request.Lease.PropertyID;
            lease.StartDate = request.Lease.StartDate;
            lease.EndDate = request.Lease.EndDate;
            lease.DateMovedIn = request.Lease.DateMovedIn;
            lease.Rent = request.Lease.Rent;
            lease.Deposit = request.Lease.Deposit;
            lease.Status = request.Lease.Status ?? LeaseStatus.Active;
            lease.PaymentFrequency = request.Lease.PaymentFrequency;
            lease.LeaseType = request.Lease.LeaseType;
            lease.TenantID = request.Lease.TenantID;
            lease.LandlordID = request.Lease.LandlordID;
            lease.PenaltyPolicyID = request.Lease.PenaltyPolicyID;
            lease.Commission = request.Lease.Commission;
            lease.UpdatedAt = DateTime.UtcNow;
            lease.LeaseNotes = request.Lease.LeaseNotes;
            lease.DisputeNotes = request.Lease.DisputeNotes;

            // 🔒 Enforce domain rules
            LeaseGuard.Validate(lease);

            // Update Deposit if exists
            if (lease.DepositMaster != null)
            {
                lease.DepositMaster.ExpectedAmount = lease.Deposit;
                lease.DepositMaster.BalanceAmount =
                    lease.Deposit - lease.DepositMaster.PaidAmount;
            }

            await _db.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
