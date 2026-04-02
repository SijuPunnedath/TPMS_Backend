using MediatR;
using TPMS.Application.Common.Models;
using TPMS.Application.Features.Disputes.DTOs;
using TPMS.Domain.Enums;

namespace TPMS.Application.Features.Disputes.Queries;

public record GetAllDisputesQuery(
    int PageNumber = 1,
    int PageSize = 10,
    DisputeStatus? Status = null,
    DisputePriority? Priority = null
) : IRequest<ApiResponse<PagedResult<DisputeListDto>>>;