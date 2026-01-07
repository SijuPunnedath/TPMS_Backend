using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.Penaltyploicy.DTOs;
using TPMS.Application.Features.Penaltyploicy.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Penaltyploicy.Handlers;

public class GetPenaltyPolicyByIdHandler : IRequestHandler<GetPenaltyPolicyByIdQuery, PenaltyPolicyDto?>
{
    private readonly TPMSDBContext _db;
    public GetPenaltyPolicyByIdHandler(TPMSDBContext db) => _db = db;

    public async Task<PenaltyPolicyDto?> Handle(GetPenaltyPolicyByIdQuery request, CancellationToken cancellationToken)
    {
        return await _db.PenaltyPolicies
            .Where(p => p.PenaltyPolicyID == request.PolicyId)
            .Select(p => new PenaltyPolicyDto
            {
                PenaltyPolicyID = p.PenaltyPolicyID,
                Name = p.Name,
                Description = p.Description,
                FixedAmount = p.FixedAmount,
                PercentageOfRent = p.PercentageOfRent,
                GracePeriodDays = p.GracePeriodDays,
                IsActive = p.IsActive
            })
            .FirstOrDefaultAsync(cancellationToken);
    }    
}