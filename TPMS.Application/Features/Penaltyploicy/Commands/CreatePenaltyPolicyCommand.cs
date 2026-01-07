using MediatR;
using TPMS.Application.Features.Penaltyploicy.DTOs;

namespace TPMS.Application.Features.Penaltyploicy.Commands;

public class CreatePenaltyPolicyCommand : IRequest<int>
{
    public CreatepenaltyDto Policy { get; set; }
}