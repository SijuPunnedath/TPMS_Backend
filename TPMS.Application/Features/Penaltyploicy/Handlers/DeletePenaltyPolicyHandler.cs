using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.Penaltyploicy.Commands;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Penaltyploicy.Handlers;

public class DeletePenaltyPolicyHandler : IRequestHandler<DeletePenaltyPolicyCommand, bool>
{
    private readonly TPMSDBContext _db;
    public DeletePenaltyPolicyHandler(TPMSDBContext db) => _db = db;

    public async Task<bool> Handle(DeletePenaltyPolicyCommand request, CancellationToken cancellationToken)
    {
        var policy = await _db.PenaltyPolicies
            .FirstOrDefaultAsync(p => p.PenaltyPolicyID == request.PolicyId, cancellationToken);

        if (policy == null) return false;

        _db.PenaltyPolicies.Remove(policy);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }  
}