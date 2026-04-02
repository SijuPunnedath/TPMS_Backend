using MediatR;
using TPMS.Application.Common.Models;
using TPMS.Application.Features.Disputes.DTOs;

namespace TPMS.Application.Features.Disputes.Queries;

public record GetDisputeByIdQuery(int Id) : IRequest<ApiResponse<DisputeDto>>;