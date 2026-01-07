using MediatR;
using TPMS.Application.Features.Penaltyploicy.DTOs;

namespace TPMS.Application.Features.Penaltyploicy.Queries;

public class GetPenaltyPolicyByIdQuery : IRequest<PenaltyPolicyDto?>
{
    public int PolicyId { get; set; } 
}