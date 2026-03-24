using MediatR;

namespace TPMS.Application.Features.Disputes.Commands;

public record AssignDisputeCommand(int Id, int AssignedToUserId) : IRequest;