using MediatR;

namespace TPMS.Application.Features.Penaltyploicy.Commands;

public class DeletePenaltyPolicyCommand : IRequest<bool>
{
    public int PolicyId { get; set; } 
}