using TPMS.Application.Common.Models;
using TPMS.Application.Features.Disputes.DTOs;

namespace TPMS.Application.Features.Disputes.Queries;

using MediatR;

public record GetDisputeDashboardQuery() 
    : IRequest<ApiResponse<DisputeDashboardDto>>;