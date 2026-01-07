using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.Penaltyploicy.Commands;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Penaltyploicy.Handlers;

public class UpdatePenaltyPolicyHandler : IRequestHandler<UpdatePenaltyPolicyCommand, bool>
{
    private readonly TPMSDBContext _db;
    public UpdatePenaltyPolicyHandler(TPMSDBContext db) => _db = db;

    public async Task<bool> Handle(UpdatePenaltyPolicyCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Policy;

        var policy = await _db.PenaltyPolicies
            .FirstOrDefaultAsync(p => p.PenaltyPolicyID == dto.PenaltyPolicyID, cancellationToken);

        if (policy == null) return false;

        policy.Name = dto.Name;
        policy.Description = dto.Description;
        policy.FixedAmount = dto.FixedAmount;
        policy.PercentageOfRent = dto.PercentageOfRent;
        policy.GracePeriodDays = dto.GracePeriodDays;
        policy.IsActive = dto.IsActive;

        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }

}