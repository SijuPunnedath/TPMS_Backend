using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.Lookups.DTOs;
using TPMS.Application.Features.Lookups.Queries;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Lookups.Handlers;

public class GetPenaltyPolicyLookupHandler : IRequestHandler<GetPenaltyPolicyLookupQuery, List<PenaltyPolicyLookupDto>>
{
    private readonly TPMSDBContext _db;
    public GetPenaltyPolicyLookupHandler(TPMSDBContext db) => _db = db;

    public async Task<List<PenaltyPolicyLookupDto>> Handle(GetPenaltyPolicyLookupQuery request, CancellationToken cancellationToken)
    {
        return await _db.PenaltyPolicies
            .Where(p => p.IsActive)
            .OrderBy(p => p.Name)
            .Select(p => new PenaltyPolicyLookupDto
            {
                PenaltyPolicyID = p.PenaltyPolicyID,
                Name = p.Name
            })
            .ToListAsync(cancellationToken);
    } 
}