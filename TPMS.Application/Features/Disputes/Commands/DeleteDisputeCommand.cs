using MediatR;
using TPMS.Application.Common.Models;

namespace TPMS.Application.Features.Disputes.Commands;

public record DeleteDisputeCommand(int Id, int UserId) 
    : IRequest<ApiResponse<bool>>;
//public record DeleteDisputeCommand(int Id, int UserId) : IRequest;