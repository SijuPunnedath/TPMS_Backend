using MediatR;
using TPMS.Application.Common.Models;
using TPMS.Application.Features.Leases.DTOs;
using TPMS.Domain.Enums;

namespace TPMS.Application.Features.Leases.Queries;

public class GetLeasesByTypePagedQuery : IRequest<PagedResult<LeaseWithSearchTermDto>>
{
    public LeaseType LeaseType { get; set; }  // Inbound / Outbound
    public string? SearchTerm { get; set; }

    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}