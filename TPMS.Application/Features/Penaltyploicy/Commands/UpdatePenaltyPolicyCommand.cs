using MediatR;
using TPMS.Application.Features.Penaltyploicy.DTOs;
namespace TPMS.Application.Features.Penaltyploicy.Commands;

public class UpdatePenaltyPolicyCommand : IRequest<bool>
{
    public PenaltyPolicyDto Policy { get; set; }  
}