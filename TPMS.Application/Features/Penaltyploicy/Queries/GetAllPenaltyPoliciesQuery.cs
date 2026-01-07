using System.Collections.Generic;
using MediatR;
using TPMS.Application.Features.Penaltyploicy.DTOs;

namespace TPMS.Application.Features.Penaltyploicy.Queries;

public class GetAllPenaltyPoliciesQuery : IRequest<IEnumerable<PenaltyPolicyDto>> 
{
    
}