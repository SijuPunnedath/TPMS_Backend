using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.Penaltyploicy.DTOs;
using TPMS.Application.Features.Penaltyploicy.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Penaltyploicy.Handlers;

public class GetAllPenaltyPoliciesHandler : IRequestHandler<GetAllPenaltyPoliciesQuery, IEnumerable<PenaltyPolicyDto>>
{
    private readonly TPMSDBContext _db;
    public GetAllPenaltyPoliciesHandler(TPMSDBContext db) => _db = db;

    public async Task<IEnumerable<PenaltyPolicyDto>> Handle(GetAllPenaltyPoliciesQuery request, CancellationToken cancellationToken)
    {
        return await _db.PenaltyPolicies
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
            .ToListAsync(cancellationToken);
    }   
}