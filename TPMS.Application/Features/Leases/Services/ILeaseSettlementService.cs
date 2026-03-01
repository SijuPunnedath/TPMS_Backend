using System.Threading;
using System.Threading.Tasks;
using TPMS.Domain.Entities;

namespace TPMS.Application.Features.Leases.Services;

public interface ILeaseSettlementService
{
    Task<LeaseSettlement> CreateSettlementAsync(
        int leaseId,
        decimal penaltyAmount,
        decimal damageCharges,
        CancellationToken cancellationToken);

    Task CompleteSettlementAsync(
        int settlementId,
        CancellationToken cancellationToken);
}