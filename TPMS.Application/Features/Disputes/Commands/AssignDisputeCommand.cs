using MediatR;
using TPMS.Application.Common.Models;

namespace TPMS.Application.Features.Disputes.Commands;

public record AssignDisputeCommand(int Id, int AssignedToUserId) : IRequest<ApiResponse<bool>>;